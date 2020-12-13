using Decanat.Models.DecanatModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Decanat.DAO
{
    public class StepDAO: AbstractDAO
    {
        public string getStepName(int id)
        {
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            string s = "";
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Name FROM Step WHERE Id=@Id",Connection);
                cmd.Parameters.Add(new SqlParameter("@Id", id));
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    s = Convert.ToString(reader["Name"]);
                    return s;
                }
            }
            catch(Exception e)
            {
                loger.Error("Произошла ошибка при запросе названия этапа");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return s;
        }

        public bool Add(Step step)
        {
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            bool result = true;
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Step(Name, Date, Comment, PlanId) VALUES (@Name, @Date, @Comment, @PlanId)", Connection);
                cmd.Parameters.Add(new SqlParameter("@Name", step.name));
                cmd.Parameters.Add(new SqlParameter("@Date", step.date));
                cmd.Parameters.Add(new SqlParameter("@Comment", step.comment));
                cmd.Parameters.Add(new SqlParameter("@PlanId", step.planId));
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                result = false;
                loger.Error("Произошла ошибка при добавлении этапа");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return result;
        }

        public List<Step> getStepsByPlanId(int id)
        {
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            List<Step> steps = new List<Step>();
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Step WHERE PlanId=@id", Connection);
                cmd.Parameters.Add(new SqlParameter("@id", id));
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int stepid = Convert.ToInt32(reader["Id"]);
                    string stepName = Convert.ToString(reader["Name"]);
                    DateTime stepDate = Convert.ToDateTime(reader["Date"]);
                    string stepComment = Convert.ToString(reader["Comment"]);
                    int planId = Convert.ToInt32(reader["PlanId"]);
                    steps.Add(new Step(stepid, stepName, stepDate, stepComment, planId));

                }
            }
            catch (Exception e)
            {
                loger.Error("Произошла ошибка при запросе всех этапов плана");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return steps;
        }
        //Редактирование этапа
        public bool edit(Step step)
        {
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            bool result = true;
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Step SET Name = @Name, Date =@Date, Comment = @Comment WHERE Id = @id" , Connection);
                cmd.Parameters.Add(new SqlParameter("@Name", step.name));
                cmd.Parameters.Add(new SqlParameter("@Date", step.date));
                cmd.Parameters.Add(new SqlParameter("@Comment", step.comment));
                cmd.Parameters.Add(new SqlParameter("@id", step.id));
                cmd.ExecuteNonQuery();
                loger.Info("Успешное изменение данных об этапе ПГ");
            }
            catch(Exception e)
            {
                result = false;
                loger.Error("Произошла ошибка при изменении данных этапа");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return result;

        }

        //Запрос информации о этапе
        public Step getStepsInfo(int id)
        {
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            Step step = new Step();
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Step WHERE Id=@id", Connection);
                cmd.Parameters.Add(new SqlParameter("@id", id));
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    step.id = Convert.ToInt32(reader["Id"]);
                    step.name = Convert.ToString(reader["Name"]);
                    step.date = Convert.ToDateTime(reader["Date"]);
                    step.comment = Convert.ToString(reader["Comment"]);
                    step.planId = Convert.ToInt32(reader["PlanId"]);

                }
            }
            catch (Exception e)
            {
                loger.Error("Произошла ошибка при запросе информации об этапе");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return step;
        }
    }
}