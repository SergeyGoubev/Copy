using Decanat.Models.DecanatModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Decanat.DAO
{//Дообален класс
    public class PlanDAO: AbstractDAO
    {
        //Добавление нового плана-графика
        public bool add(Plan plan)
        {
            bool result = true;
            Connect();
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Plan (GruppaId, Status) VALUES " + "(@GruppaId, @Status)", Connection);
                cmd.Parameters.Add(new SqlParameter("@Surname", plan.gpoupId));
                cmd.Parameters.Add(new SqlParameter("@Status", plan.status));
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
            result = false;
            loger.Error("Произошла ошибка при добавлении плана-графика");
            loger.Trace(e.StackTrace);
        }
        finally
        {
            Disconnect();
        }
        return result;
    }
        //Поиск планов по статусу
        public List<Plan> showPlansByStatus(int status)
        {
            List<Plan> plans = new List<Plan>();
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Plan WHERE Status = @Status", Connection);
                cmd.Parameters.Add(new SqlParameter("@Status", status));
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    int gruoupId = Convert.ToInt32(reader["GruppaId"]);
                    int st = Convert.ToInt32(reader["Status"]);
                    plans.Add(new Plan(id, gruoupId, st));
                }
            }
            catch(Exception e)
            {
                loger.Error("Произошла ошибка при запросе планов-графиков");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return plans;
        }

        //Вывод информации о ПГ
        public Plan showPlanInfo(int id)
        {
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            Plan plan = new Plan();
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Plan WHERE Id=@Id", Connection);
                cmd.Parameters.Add(new SqlParameter("@Id", id));
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    plan.id = Convert.ToInt32(reader["Id"]);
                    plan.gpoupId = Convert.ToInt32(reader["GruppaId"]);
                    plan.status = Convert.ToInt32(reader["Status"]);
                } 
            }
            catch (Exception e)
            {
                loger.Error("Произошла ошибка при запросе плана-графика");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return plan;
        }

        public Plan showPlanInfoByGropId(int groupId)
        {
            Connect();
            Plan plan = new Plan();
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            try
            {
                
                SqlCommand cmd = new SqlCommand("SELECT * FROM Plan WHERE GruppaId=@GruppaId", Connection);
                cmd.Parameters.Add(new SqlParameter("@GruppaId", groupId));
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    plan.id = Convert.ToInt32(reader["Id"]);
                    plan.gpoupId = Convert.ToInt32(reader["GruppaId"]);
                    plan.status = Convert.ToInt32(reader["Status"]);
                    
                }
            }
            catch (Exception e)
            {
                loger.Error("Произошла ошибка при запросе плана-графика");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return plan;
        }

        //Сменить статус плана
        public bool setStatus(int id, int status)
        {
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            bool result = true;
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("UPDATE Plan SET Status = @status WHERE Id = @id", Connection);
                cmd.Parameters.Add(new SqlParameter("@Id", id));
                cmd.Parameters.Add(new SqlParameter("@status", status));
                cmd.ExecuteNonQuery();
                loger.Info("Успешное изменение статуса ПГ");
            }
            catch(Exception e)
            {
                result = false;
                loger.Error("Произошла ошибка при изменении статуса ПГ");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return result;
        }

        public bool isPlanAproved(int id)
        {
            bool result = false;
            Connect();
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            try
            {
                
                SqlCommand cmd = new SqlCommand("SELECT Status FROM Plan WHERE Id = @Id", Connection);
                cmd.Parameters.Add(new SqlParameter("@Id", id));
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int status = Convert.ToInt32(reader["Status"]);
                    if (status == 2 || status == 5)
                    {
                        result = true;
                    } else
                    {
                        result = false;
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                loger.Error("Произошла ошибка при Запросе статуса плана");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return result;
        }
    }
}