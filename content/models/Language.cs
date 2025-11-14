using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;

namespace GamesShop.content.models
{
    public class Language
    {
        [Key]
        [Column("LanguageID")]
        public int LanguageID { get; set; }

        [Column("LanguageName")]
        public string LanguageName { get; set; }

        [Column("LanguageCode")]
        public string LanguageCode { get; set; }

        public virtual ICollection<GameLanguage> GameLanguages { get; set; }

        public Language()
        {
            GameLanguages = new HashSet<GameLanguage>();
        }
    }
}
