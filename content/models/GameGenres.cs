using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.content.models
{
    public class GameGenres
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("GameID")]
        public int GameID { get; set; }

        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }

        [Column("GenreID")]
        public int GenreID { get; set; }

        [ForeignKey("GenreID")]
        public virtual Genre Genre { get; set; }
    }
}
