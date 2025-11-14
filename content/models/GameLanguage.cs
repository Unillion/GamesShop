using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.content.models
{
    public class GameLanguage
    {
        [Key]
        [Column("GameLanguageID")]
        public int GameLanguageID { get; set; }

        [Column("GameID")]
        public int GameID { get; set; }

        [ForeignKey("GameID")]
        public virtual Game Game { get; set; }

        [Column("LanguageID")]
        public int LanguageID { get; set; }

        [ForeignKey("LanguageID")]
        public virtual Language Language { get; set; }
    }
}
