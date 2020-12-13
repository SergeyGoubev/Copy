using Decanat.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decanat.Models.DecanatModels
{
    public class Answer
    {
        public int id { get; set; }
        public string link { get; set; }
        public int mark { get; set; }
        public DateTime markDate { get; set; }
        public DateTime answerDate { get; set; }
        public int vkrId { get; set; }
        public int stepid { get; set; } 
        public string stepName
        {
            get
            {
                StepDAO sDAO = new StepDAO();
                return sDAO.getStepName(stepid);
            }
        }
        public string vkrName
        {
            get
            {
                VkrDAO vDAO = new VkrDAO();
                return vDAO.getVKRName(vkrId);
            }
            
        }
        public DateTime getDate
        {
            get
            {
                StepDAO sDAO = new StepDAO();
                return sDAO.getStepsInfo(stepid).date;
            }
        }
        public int gruppaId { get; set; }
        public string gruppaName
        {
            get
            {
                GruppaDAO gDAO = new GruppaDAO();
                return gDAO.getGruppaName(gruppaId);
            }
        }
        public int status { get; set; }
        //0 - Не представлен
        //1 - Представлен
        //2 - Оценён
        //3 - Отправлен на исправление
        //4 - Просрочен
        //5 - Добавлено время
        //6 - Предствален с опозданием
        
        public string statusName
        {
                get {
                switch (this.status)
                {
                    case 0:
                        return "Не представлен";
                    case 1:
                        return "Представлен";
                    case 2:
                        return "Оценён";
                    case 3:
                        return "Отправлен на исправление";
                    case 4:
                        return "Просрочен";
                    case 5:
                        return "Добавленно время";
                    case 6:
                        return "Представлен с опозданием";
                    default:
                        return "error 404";
                }
            }
        }


        //****************************************************************
        //Конструкторы
        //****************************************************************
        public Answer(int vkrId, int stepId) 
        {
            this.vkrId = vkrId;
            this.stepid = stepId;
        }
        

        public Answer(int id)
        {
            this.id = id;
        }

        public Answer(int id, string link,  int vkrId)
        {
            this.id = id;
            this.link = link;
            this.vkrId = vkrId;
        }
        public Answer(int id, string link, int vkrId, DateTime answerDate)
        {
            this.id = id;
            this.link = link;
            this.vkrId = vkrId;
            this.answerDate = answerDate;
        }
        public Answer()
        {

        }

        public Answer(int id, int vkrId, int stepid, string link, int mark, int status, DateTime answerDate, DateTime markDate)
        {
            this.id = id;
            this.vkrId = vkrId;
            this.stepid = stepid;
            this.link = link;
            this.mark = mark;
            this.status = status;
            this.answerDate = answerDate;
            this.markDate = markDate;
        }
        public Answer(int id, int vkrId, int stepid, string link, int status)
        {
            this.id = id;
            this.vkrId = vkrId;
            this.stepid = stepid;
            this.link = link;
            this.mark = mark;
            this.status = status;
            this.answerDate = answerDate;
            this.markDate = markDate;
        }

        public Answer(int id, int stepid, int status)
        {
            this.id = id;
            this.status = status;
            this.stepid = stepid;

        }
    }
}