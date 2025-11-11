using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.content.models
{
    public class User
    {

        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("Username")]
        public string Username { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [NotMapped]
        public string Password { get; set; }

        [Column("PasswordHash")]
        public string PasswordHash { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Library Library { get; set; }
        public virtual Bill Bill { get; set; }

        public User() { }

        public User(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }
    }
}
