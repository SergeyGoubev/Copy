using Decanat.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decanat.Models.DecanatModels
{

    public class Step
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
  
        public string comment { get; set; }
        public int planId { get; set; }
        
        public bool isPlanAproved
        {
            get
            {
                PlanDAO pDAO = new PlanDAO();
                return pDAO.isPlanAproved(planId);
            }
        }
        //*******************************************************************
        //Конструкторы
        //*******************************************************************
        public Step()
        {

        }

        public Step(int id, string name, DateTime date, string comment, int planId)
        {
            this.id = id;
            this.name = name;
            this.date = date;
            this.comment = comment;
            this.planId = planId;
        }




    }
}