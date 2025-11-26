using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.content.models
{
    public class GameStatistics
    {

        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("GameID")]
        public int GameID { get; set; }

        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }

        [Column("TotalPurchases")]
        public int TotalPurchases { get; set; }

        [Column("LastUpdated")]
        public DateTime LastUpdated { get; set; }

        [Column("UsersKeyRefreshes")]
        public int UsersKeyRefreshes { get; set; }

        public GameStatistics() { }
    }
}
