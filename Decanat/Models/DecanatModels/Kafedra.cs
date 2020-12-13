using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decanat.Models.DecanatModels
{
    public class Kafedra
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }

        public Kafedra()
        {

        }

        public Kafedra(int id, string name, string email)
        {
            this.id = id;
            this.name = name;
            this.email = email;
        }
    }
}