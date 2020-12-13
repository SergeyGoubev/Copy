using Decanat.Models.DecanatModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Decanat.DAO
{
    public class VkrDAO: AbstractDAO
    {

        //Получить Название ВКР
        public string getVKRName(int id)
        {
            string s = "";
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("Select Theme from VKR where id = @id", Connection);
                cmd.Parameters.Add(new SqlParameter("@id", id));
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    s = Convert.ToString(reader["Theme"]);
                }
            }
            catch(Exception e)
            {
                loger.Error("Произошла ошибка при получении названия ВКР");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return s;
        }
        //Запрос всех ВКР
        public List<VKR> getAllVKR()
        {
            Connect();
            List<VKR> works = new List<VKR>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM VKR",Connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string theme = Convert.ToString(reader["Theme"]);
                works.Add(new VKR(theme));
            }
            Disconnect();
            return works;
        }

        //Поиск ВКР по ID
        public VKR getVKRbyId(int id)
        {
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            VKR vkr = new VKR();
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM VKR WHERE Id = @id", Connection);
                cmd.Parameters.Add(new SqlParameter("@id", id));
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vkr.id = Convert.ToInt32(reader["Id"]);
                    vkr.theme = Convert.ToString(reader["Theme"]);
                    vkr.studentId = Convert.ToInt32(reader["StudentId"]);
                    vkr.teacherId = Convert.ToInt32(reader["TeacherId"]);
                    vkr.status = Convert.ToInt32(reader["Status"]);

                }

            }
            catch(Exception e)
            {
                loger.Error("Произошла ошибка поиске ВКР");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return vkr;
        }

        //Поиск ВКР по студенту
        public VKR getVKRbyStudent(int studentid)
        {
            VKR vkr = new VKR();
            Connect();
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);

            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM VKR WHERE StudentId=@stId", Connection);
                cmd.Parameters.Add(new SqlParameter("@stId", studentid));
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vkr.id = Convert.ToInt32(reader["Id"]);
                    vkr.theme = Convert.ToString(reader["Theme"]);
                    vkr.studentId = Convert.ToInt32(reader["StudentId"]);
                    vkr.teacherId = Convert.ToInt32(reader["TeacherId"]);
                    
                }
                return vkr;
            }
            catch(Exception e)
            {
                loger.Error("Произошла ошибка поиске ВКР");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return vkr;
        }

        //Запрос только что добавленный ВКР
        public VKR getNewVKR(int studentId, int teacherId, int PlanId)
        {
            VKR newVKR = new VKR();
            Connect();
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Id FROM VKR WHERE StudentId = @StudentId AND TeacherId = @TeacherId AND PlanId = @PlanId", Connection);
                cmd.Parameters.Add(new SqlParameter("@StudentId", studentId));
                cmd.Parameters.Add(new SqlParameter("@TeacherId", teacherId));
                cmd.Parameters.Add(new SqlParameter("@PlanId", PlanId));
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    newVKR.id = id;
                }
            loger.Info(newVKR.id);

            }
            catch(Exception e)
            {
                loger.Error("Произошла ошибка при добавлении поля ответа");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return newVKR;
        }
        
        //Добавление ВКР
        //Необходимо добавить в БД в Таблице ВКР ссылку на План-График
        public bool add(VKR vkr)
        {
            bool result = true;
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO VKR(Theme, StudentId, TeacherId, PlanId) VALUES (@Theme, @StudentId, @TeacherId, @PlanId)", Connection);
                cmd.Parameters.Add(new SqlParameter("@Theme", vkr.theme));
                cmd.Parameters.Add(new SqlParameter("@StudentId", vkr.studentId));
                cmd.Parameters.Add(new SqlParameter("@TeacherId", vkr.teacherId));
                cmd.Parameters.Add(new SqlParameter("@PlanId", vkr.planId));
                cmd.ExecuteNonQuery();
            loger.Info(vkr.theme + " " + vkr.teacherId + " " + vkr.studentId + " " + vkr.planId);
                StepDAO sDAO = new StepDAO();
                int tempId = getNewVKR(vkr.studentId, vkr.teacherId, vkr.planId).id;
                List<Step> steps = sDAO.getStepsByPlanId(vkr.planId);
                AnswerDAO aDAO = new AnswerDAO();
                foreach (Step item in steps)
                {
                    aDAO.add(new Answer(tempId,item.id));
                }
                StudentDAO stDAO = new StudentDAO();
                result = stDAO.setStudentVKRstat(vkr.studentId, true);


            }
            catch(Exception e)
            {
                result = false;
                loger.Error("Произошла ошибка при ВКР");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return result;
        }


        public bool setStatus(int id, int status)
        {
            bool resuln = true;
            Connect();
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE VKR SET Status=@status WHERE Id = @id", Connection);
                cmd.Parameters.Add(new SqlParameter("@id", id));
                cmd.Parameters.Add(new SqlParameter("@status", status));
                cmd.ExecuteNonQuery();
                loger.Info("Успешное изменнение статуса ВКР");
            }
            catch (Exception e)
            {
                resuln = false;
                loger.Error("Произошла ошибка при изменении статуса ВКР");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return resuln;
        }
        //проверка статуса ВКР
        public void checkStatus(int id)
        {
            bool result = true;
            AnswerDAO aDAO = new AnswerDAO();
            List<Answer> answers = aDAO.getAnswersByVKR(id);
            foreach (var item in answers)
            {
                if (item.status == 1)
                {
                    result = result && true;
                }
                else { result = result && false; break; }
            }
            if (result)
            {
                setStatus(id, 2);
            }
        }
    }
}