using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decanat.Models.DecanatModels
{
    public class Teacher
    {
        public int id { get; set; }
        public string surname { get; set; }
        public string firstName { get; set; }
        public string patronymic { get; set; }
        public string position { get; set; }
        public int kafedraId { get; set; }
        public string email { get; set; }


        public Teacher(int id, string surname, string firstName, string patr, string position, int kafedra, string email)
        {
            this.id = id;
            this.surname = surname;
            this.firstName = firstName;
            this.patronymic = patr;
            this.position = position;
            this.kafedraId = kafedra;
            this.email = email;
        }
        public Teacher( string surname, string firstName, string patr, string position, int kafedra)
        {
            this.surname = surname;
            this.firstName = firstName;
            this.patronymic = patr;
            this.position = position;
            this.kafedraId = kafedra;
        }

        public string getFIO()
        {
            return surname + " " + firstName + " " + patronymic;
        }
        public Teacher()
        {
        }
    }
}