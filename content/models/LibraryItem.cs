using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.content.models
{
    public class LibraryItem
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("LibraryID")]
        public int LibraryID { get; set; }

        [ForeignKey("LibraryID")]
        public virtual Library Library { get; set; }

        [Column("GameID")]
        public int GameID { get; set; }

        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }
    }
}
