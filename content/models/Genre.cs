using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;

namespace GamesShop.content.models
{
    public class Genre
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        

        public virtual ICollection<GameGenres> GameGenres { get; set; }

        public Genre()
        {
            GameGenres = new HashSet<GameGenres>();
        }
    }
}
