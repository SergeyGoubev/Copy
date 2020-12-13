using Decanat.Models.DecanatModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Decanat.DAO
{
    public class KafedraDAO: AbstractDAO
    {
        //Получение названия кафедры
        public string getKafedraName(int id)
        {
            Connect();
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT Name FROM Kafedra WHERE id=@id", Connection);
                cmd.Parameters.Add(new SqlParameter("@id", id));
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string name = Convert.ToString(reader["Name"]);
                    return name;
                }
            }
            catch(Exception e)
            {
                loger.Error("Произошла ошибка при запросе названия группы");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return "";
        }

        //Получение инфармации о кафедре
        public Kafedra getKafedraInfo(int id)
        {
            Kafedra kaf = new Kafedra();
            Connect();
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Kafedra WHERE id=@id", Connection);
                cmd.Parameters.Add(new SqlParameter("@id", id));
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    kaf.id = Convert.ToInt32(reader["Id"]);
                    kaf.name = Convert.ToString(reader["Name"]);
                    kaf.email = Convert.ToString(reader["Email"]);
                }
            }
            catch(Exception e)
            {
                loger.Error("Произошла ошибка при запросе инфаормации о группе");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return kaf;
        }

        //Список всех кафедр
        public List<Kafedra> getAllKafedras()
        {
            List<Kafedra> kafedras = new List<Kafedra>();
            Connect();
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Kafedra",Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string name = Convert.ToString(reader["Name"]);
                    string email = Convert.ToString(reader["email"]);
                    kafedras.Add(new Kafedra(id,name,email));
                }
                loger.Info("Успешное получение информации о всех кафедрах");
            }
            catch(Exception e)
            {
                loger.Error("Произошла ошибка при запросе инфаормации о группе");
                loger.Trace(e.StackTrace);
            }
            finally
            {
                Disconnect();
            }
            return kafedras;
        }

        public bool add(Kafedra kafedra)
        {
            bool result = true;
            loger.Info("Вызван метод " + new StackTrace(false).GetFrame(0).GetMethod().Name);
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Kafedra(Name, email) VALUES (@Name, @email)", Connection);
                cmd.Parameters.Add(new SqlParameter("@Name", kafedra.name));
                cmd.Parameters.Add(new SqlParameter("@email", kafedra.email)); ;
                cmd.ExecuteNonQuery();
                loger.Info("Успешное добавление кафедры" + kafedra.name);
            }
            catch(Exception e)
            {
                result = false;
                loger.Error("Произошла ошибка при запросе инфаормации о группе");
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