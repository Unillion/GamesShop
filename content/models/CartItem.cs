using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.content.models
{
    public class CartItem
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("CartID")]
        public int CartID { get; set; }

        [ForeignKey("CartID")]
        public virtual Cart Cart { get; set; }

        [Column("GameID")]
        public int GameID { get; set; }

        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }
    }
}
