using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decanat.Models.DecanatModels
{
    public class VKR
    {
        public int id { get; set; }
        public string theme { get; set; }
        public int teacherId { get; set; }
        public int status { get; set; }
        //1 - На выполнении
        //2 - Выполнена
        //3 - Просрочена
        //4 - Отменена
        public int studentId { get; set; }
        public int planId { get; set; }

        public VKR(string theme)
        {
            this.theme = theme;
        }

        
        public string getStatus { get
        {
                switch (status)
                {
                    case (1):
                        return "На выполнении";
                    case (2):
                        return "Выполнена";
                    case (3):
                        return "Просрочена";
                    case (4):
                        return "Отменена";
                    default:
                        return "Не удалось загрузить статус работы";
                }
            } 
        }
        
        //*****************************************************
        //Кострукторы
        //*****************************************************
        
        public VKR(int id, string theme, int studentId, int teacherId, int status)
        {
            this.id = id;
            this.theme = theme;
            this.teacherId = teacherId;
            this.studentId = studentId;
            this.status = status;
        }

        public VKR(string theme, int studentId, int teacherId, int planId)
        {
            this.theme = theme;
            this.studentId = studentId;
            this.teacherId = teacherId;
            this.planId = planId;
        }

        public VKR()
        {

        }
    }
     
}