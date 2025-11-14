using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.content.models
{
    public class Developers
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        public virtual ICollection<GameDevelopers> GameDevelopers { get; set; }

        public Developers() 
        {
            GameDevelopers = new HashSet<GameDevelopers>();
        }
    }
}
