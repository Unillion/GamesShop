using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.content.models
{
    public class UserStatistics
    {
        [Key]
        [Column("UserStatID")]
        public int UserStatID { get; set; }

        [Column("UserID")]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        [Column("TotalGamesPurchased")]
        public int TotalGamesPurchased { get; set; } = 0;

        [Column("TotalMoneySpent")]
        public decimal TotalMoneySpent { get; set; } = 0.00m;

        [Column("TotalIncomeAmount")]
        public decimal TotalIncomeAmount { get; set; } = 0.00m;

        [Column("ReviewsWritten")]
        public int ReviewsWritten { get; set; } = 0;
    }
}
