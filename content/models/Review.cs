using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.content.models
{
    public class Review
    {
        [Key]
        [Column("ID")]
        public int ReviewID { get; set; }

        [Column("UserID")]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [Column("GameID")]
        public int GameID { get; set; }

        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }

        [Column("ReviewText")]
        public string ReviewText { get; set; }

        [Column("ReviewDate")]
        public DateTime ReviewDate { get; set; } = DateTime.Now;

        [Column("Rating")]
        public int Rating { get; set; }
    }
}
