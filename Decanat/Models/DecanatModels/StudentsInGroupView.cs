using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decanat.Models.DecanatModels
{
    public class StudentsInGroupView
    {
        public List<Student> students;
        public Gruppa group;

        public StudentsInGroupView(Gruppa group, List<Student> students)
        {
            this.students = students;
            this.group = group;
        }
    }
}