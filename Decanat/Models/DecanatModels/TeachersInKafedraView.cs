using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decanat.Models.DecanatModels
{
    public class TeachersInKafedraView
    {
        public Kafedra kafedra;
        public List<Teacher> teachers;

        public TeachersInKafedraView(Kafedra kafedra, List<Teacher> teachers)
        {
            this.teachers = teachers;
            this.kafedra = kafedra;
        }
    }
}