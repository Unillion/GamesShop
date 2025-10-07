using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop
{
    public class User
    { 

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public User(string name, string email, string password)
        {
            UserName = name;
            Email = email;
            Password = password;
        }

    }
}
