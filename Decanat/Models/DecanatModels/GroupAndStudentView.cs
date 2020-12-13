using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decanat.Models.DecanatModels
{
    public class GroupAndStudentView
    {
        public Student student;
        public Gruppa gruppa;

        public GroupAndStudentView(Student student, Gruppa gruppa)
        {
            this.student = student;
            this.gruppa = gruppa;
        }
    }
}