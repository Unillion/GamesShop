using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.content.user
{
    public partial class UserManager
    {
        public static List<User> Users { get; set; } = new List<User>();
        public static User RegisterUser(string name, string email, string password)
        {
            User user = new User(name, email, password);

            Users.Add(user);
            return user;
        }
    }
}
