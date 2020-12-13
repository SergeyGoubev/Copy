using Decanat.Models.DecanatModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Decanat.DAO
{
    public class UserDAO: AbstractDAO
    {
        public List<string> getUnregistegUser()
        {
            List<string> users = new List<string>();
            string ConnectionScting = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            try{
                Connection = new SqlConnection(ConnectionScting);
                Connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM AspNetUsers WHERE  Id NOT IN (SELECT UserID FROM AspNetUserRoles)", Connection);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string email = Convert.ToString(reader["Email"]);
                    users.Add(Convert.ToString(reader["Email"]));
                }
                reader.Close();
            }
            catch(Exception e)
            {
                loger.Error("Произошла ошибка при запросе пользоователей без ролей");
                loger.Trace(e.StackTrace);
            }
            return users;
        }
    }
}