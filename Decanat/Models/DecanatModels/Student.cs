using Decanat.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decanat.Models.DecanatModels
{
    public class Student
    {
        public int id { get; set; }
        public string surname { get; set; }
        public string firstName { get; set; }
        public string patronymic { get; set; }
        public string mobileNomber { get; set; }
        public string email { get; set; }
        public int gruppaId { get; set; }
        public string gruppaName
        {
            get
            {
                GruppaDAO gDAO = new GruppaDAO();
                return gDAO.getGruppaName(this.id);
            }
        }
        public bool isHasVKR { get; set; }
        public string getIsHasVKR {
            get
            {
                if (isHasVKR) return "Уже работает над ВКР";
                else return "Данные о ВКР пока не добавлены";
            }
                }
        public int getVKR
        {
            get
            {
                VkrDAO vDAO = new VkrDAO();
                return vDAO.getVKRbyStudent(id).id;
            }
        }
        public string getFIO
        {
            get
            {
                return surname + " " + firstName + " " + patronymic;
            }
        }

        //***********************************************************************
        //Конструкторы
        //***********************************************************************
        public bool isPlanAproved
        {
            get
            {
                PlanDAO pDAO = new PlanDAO();
                if (pDAO.showPlanInfoByGropId(gruppaId).status == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public Student()
        {

        }

        public Student(int id, string surname, string firstName, string patronymic, string mobileNomber, string email, int gruppaId) 
        {
            this.id = id;
            this.surname = surname;
            this.firstName = firstName;
            this.patronymic = patronymic;
            this.mobileNomber = mobileNomber;
            this.email = email;
            this.gruppaId = gruppaId;
        }
        public Student(string surname, string firstName, string patronymic, string mobileNomber, string email, int gruppaId)
        {
            this.surname = surname;
            this.firstName = firstName;
            this.patronymic = patronymic;
            this.mobileNomber = mobileNomber;
            this.email = email;
            this.gruppaId = gruppaId;
        }
        public Student (int id, string surname, string firstName, string patronymic, string mobileNomber, string email)
        {
            this.id = id;
            this.surname = surname;
            this.firstName = firstName;
            this.patronymic = patronymic;
            this.mobileNomber = mobileNomber;
            this.email = email;
        }



        
    }
}