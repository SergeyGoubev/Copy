using Decanat.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decanat.Models.DecanatModels
{
    public class Plan
        
    {
        public int id { get; set; }
        public int gpoupId { get; set; }
        public int status { get; set; }

        public string getStatus
        {
            get
            {
                switch (this.status)
                {
                    case 0:
                        return "Создан";
                    case 1:
                        return "На ваполнении";
                    case 2:
                        return "На исправлении";
                    case 3:
                        return "Отменён";
                    case 4:
                        return "Выполнен";
                    case 5:
                        return "На одобрении";
                    default:
                        return "Ошибка";

                }
            }
          
        }

        public string getGroupName
        {
            get
            {
                GruppaDAO gDAO = new GruppaDAO();
                return gDAO.getGruppaName(this.gpoupId);
            }
        }


        //************************************************************
        //Конструкторы
        //************************************************************

        public Plan()
        {

        }

       public Plan(int groupId, int status)
        {
            this.gpoupId = groupId;
            this.status = status;
        }

        public Plan(int id, int groupId, int status)
        {
            this.id = id;
            this.gpoupId = groupId;
            this.status = status; 
        }

        public Plan(int groupId)
        {
            gpoupId = groupId;
        }

    }
}
 
 