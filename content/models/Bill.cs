using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.content.models
{
    public class Bill
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("UserID")]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public virtual ICollection<Balance> Balances { get; set; }

        public Bill()
        {
            Balances = new HashSet<Balance>();
        }
    }
}
