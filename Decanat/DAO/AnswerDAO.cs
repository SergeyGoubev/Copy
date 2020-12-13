using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Decanat.Models.DecanatModels;
using NLog;

namespace Decanat.DAO
{
    public class AnswerDAO : AbstractDAO
    {

        //Просмотр последних ответов для преподавателя
        public List<Answer> getLastAnswers(int id)
        {
            List<Answer> lastAnswers = new List<Answer>();
            Connect();
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            try
           {
               SqlCommand cmd = new SqlCommand("SELECT * FROM Answer, VKR WHERE (Answer.VKRId=VKR.Id AND VKR.TeacherId=@id AND Answer.Status=1)", Connection);
                cmd.Parameters.Add(new SqlParameter("@id", id));
                SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int tempId = Convert.ToInt32(reader["Id"]);
                string tempLink = Convert.ToString(reader["Link"]);
                int tepVkrId = Convert.ToInt32(reader["VKRId"]);
                DateTime tempAnswerDate = Convert.ToDateTime(reader["AnswerDate"]);
                    lastAnswers.Add(new Answer(tempId,tempLink,tepVkrId,tempAnswerDate));
                }
                loger.Info("Успешный вывод информации о последних ответах");
            }
            catch (Exception e)
            {
                //Обработка ошибки
                loger.Error("Произошла ошибка при получении информации о последних ответах");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return lastAnswers;

        }

        //Поставить оценку
        public bool setMark(int id, int mark)
        {
            bool result = true;
            Connect();
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Answer SET Mark = @mark, markDate = @mDate, Status = 2 WHERE Id=@id",Connection);
                cmd.Parameters.Add(new SqlParameter("@id", id));
                cmd.Parameters.Add(new SqlParameter("@mark", mark));
                cmd.Parameters.Add(new SqlParameter("@mDate", DateTime.Now));
                cmd.ExecuteNonQuery();
                loger.Info("Успешный выставление оценки");
                //setStatus(id, 2); 
            }
            catch(Exception e)
            {
                result = false;
                loger.Error("Произошла ошибка при Выставлении оценки");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();

            }
            return result;
        }
        
        //Просмотреть ответ
        public Answer getInfo(int id)
        {
            Answer ans = new Answer();
            Connect();
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Answer WHERE Id=@id", Connection);
                cmd.Parameters.Add(new SqlParameter("@id", id));
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ans.id = Convert.ToInt32(reader["id"]);
                    ans.status = Convert.ToInt32(reader["Status"]);
                    if (ans.status == 1) { ans.answerDate = Convert.ToDateTime(reader["AnswerDate"]); }
                    ans.link = Convert.ToString(reader["Link"]);
                    ans.vkrId = Convert.ToInt32(reader["VKRId"]);
                    ans.stepid = Convert.ToInt32(reader["StepId"]);
                    if (ans.status == 2)
                    {
                        ans.mark = Convert.ToInt32(reader["Mark"]);
                        ans.markDate = Convert.ToDateTime(reader["markDate"]);
                        loger.Info("Успешный вывод информации об ответе");
                        return ans;
                    }
                    else
                    {
                        loger.Info("Успешный вывод информации об ответе"); ;
                        return ans;
                    }
                    
                }
            }
            catch(Exception e)
            {
                loger.Error("Произошла ошибка при получении информации об ответе");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return ans;
        }

        //Смена статуса
        public bool setStatus(int id, int status)
        {
            bool resuln = true;
            Connect();
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Answer SET Status=@status WHERE Id = @id", Connection);
                cmd.Parameters.Add(new SqlParameter("@id", id));
                cmd.Parameters.Add(new SqlParameter("@status", status));
                cmd.ExecuteNonQuery();
                loger.Info("УУУУУУУУспешное изменнеие статуса ответа");
            }
            catch(Exception e)
            {
                resuln = false;
                loger.Error("Произошла ошибка при изменении статуса ответа");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return resuln;
        }

        //Добавление поля для ответа
        public bool add(Answer ans)
        {
            bool result = true;
            Connect();
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Answer(VKRId, StepId) VALUES (@VKRId, @StepId)", Connection );
                cmd.Parameters.Add(new SqlParameter("@VKRId", ans.vkrId));
                cmd.Parameters.Add(new SqlParameter("@StepId", ans.stepid));
                cmd.ExecuteNonQuery();
                loger.Info("Успешное добавление поля для ответа");
            }
            catch(Exception e)
            {
                result = false;
                loger.Error("Произошла ошибка при добавлении поля ответа");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return result;
        }

        //Предоставление ответа
        public bool sendAnswer(int id, string link)
        {
            bool result = true;
            Connect();
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Answer SET Link = @Link, AnswerDate = @aDate WHERE Id=@Id", Connection);
                cmd.Parameters.Add(new SqlParameter("@Id",id));
                cmd.Parameters.Add(new SqlParameter("@Link", link));
                cmd.Parameters.Add(new SqlParameter("@aDate", DateTime.Now));
                cmd.ExecuteNonQuery();
                if (getInfo(id).status == 0 || getInfo(id).status == 3)
                {
                    setStatus(id, 1);
                }
                else if (getInfo(id).status==5)
                {
                    setStatus(id, 6);
                }
                loger.Info("Успешное отправка ответа");

            }
            catch(Exception e)
            {
                result = false;
                loger.Error("Произошла ошибка при отправки ответа");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return result;
        }
        
        //Список всех ответо для ВКР
        public List<Answer> getAnswersByVKR(int VKRId)
        {
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            List<Answer> answers = new List<Answer>();
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Answer WHERE VKRId = @VKRId", Connection);
                cmd.Parameters.Add(new SqlParameter("@VKRId", VKRId));
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    int stepId = Convert.ToInt32(reader["StepId"]);
                    string link = Convert.ToString(reader["Link"]);
                    int status = Convert.ToInt32(reader["Status"]);
                    int vkrid = Convert.ToInt32(reader["VKRId"]);
                    answers.Add(new Answer(id, vkrid, stepId, link, status));
                }
                loger.Info("Успешный запрос информации об ответах");
            }
            catch(Exception e)
            {
                loger.Error("Произошла ошибка при Запросе информации об ответах");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return answers;
        }

        //Список всех ответо для ВКР
        public List<Answer> getAnswersByStep(int PlanId)
        {
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            List<Answer> answers = new List<Answer>();
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Answer WHERE StepId = @StepId", Connection);
                cmd.Parameters.Add(new SqlParameter("@StepId", PlanId));
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    int stepId = Convert.ToInt32(reader["StepId"]);
                    string link = Convert.ToString(reader["Link"]);
                    int status = Convert.ToInt32(reader["Status"]);
                    int vkrid = Convert.ToInt32(reader["VKRId"]);
                    answers.Add(new Answer(id, vkrid, stepId, link, status));
                }
                loger.Info("Успешный запрос информации об ответах");
            }
            catch (Exception e)
            {
                loger.Error("Произошла ошибка при Запросе информации об ответах");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return answers;
        }

       public List<Answer> getAllAnswers()
        {
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            List<Answer> answers = new List<Answer>();
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Answer ", Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    int stepId = Convert.ToInt32(reader["StepId"]);
                    int status = Convert.ToInt32(reader["Status"]);
                    answers.Add(new Answer(id, stepId, status));
                }
                loger.Info("Успешный запрос информации об ответах");
            }
            catch (Exception e)
            {
                loger.Error("Произошла ошибка при Запросе информации о всех ответах");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return answers;
        }

        //Проверка просроченных ответов
        public bool checkAllAnswers()
        {
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            bool result = true;
            StepDAO sDAO = new StepDAO();
            try
            {
                List<Answer> answers = getAllAnswers();
                foreach (var item in answers)
                {
                    loger.Info(item.status + " " + sDAO.getStepsInfo(item.stepid).date + " " + DateTime.Now);
                    if ((item.status == 0 | item.status == 3) & sDAO.getStepsInfo(item.stepid).date < DateTime.Now)
                    {
                        loger.Info("Время сдачи меньше текущего, да и ответ не представлен");
                        setStatus(item.id, 4);
                    }
                }
            }
            catch(Exception e)
            {
                result = false;
                loger.Error("Произошла ошибка при поиске просроченных ответов");
                loger.Trace(e.StackTrace);
            }
            return result;
        }
    }

    

}