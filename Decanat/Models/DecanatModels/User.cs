using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Decanat.Models.DecanatModels
{
    public class User
    {
        public string id;
        public string email;

        public User(string id, string email)
        {
            this.id = id;
            this.email = email;
        }
    }
}