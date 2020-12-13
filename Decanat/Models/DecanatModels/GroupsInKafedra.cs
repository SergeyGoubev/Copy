using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decanat.Models.DecanatModels
{
    public class GroupsInKafedra
    {
        public List<Gruppa> groups;
        public Kafedra kafedra;

        public GroupsInKafedra(List<Gruppa> groups, Kafedra kafedra)
        {
            this.groups = groups;
            this.kafedra = kafedra;
        }
    }
}