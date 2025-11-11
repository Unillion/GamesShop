using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.content.models
{
    public class Game
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }

        [Column("Title")]
        public string Title { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Genre")]
        public string Genre { get; set; }

        [Column("Price", TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Column("ReleaseDate")]
        public DateTime ReleaseDate { get; set; }

        [Column("Rating")]
        public int Rating { get; set; }

        [Column("Logo")]
        public string Logo { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<LibraryItem> LibraryItems { get; set; }

        public Game()
        {
            CartItems = new HashSet<CartItem>();
            LibraryItems = new HashSet<LibraryItem>();
        }

        public Game(string title, string description, string genre, decimal price, DateTime releaseDate, int rating, string logo)
        {
            Title = title;
            Description = description;
            Genre = genre;
            Price = price;
            ReleaseDate = releaseDate;
            Rating = rating;
            Logo = "content/Images/" + logo;
            CartItems = new HashSet<CartItem>();
            LibraryItems = new HashSet<LibraryItem>();
        }

        public string GetFormattedPrice() => Price.ToString("F2") + " $";
        public string GetFormattedReleaseDate() => ReleaseDate.ToString("dd.MM.yyyy");
        public string GetRatingStars() => new string('★', Rating) + new string('☆', 5 - Rating);
    }
}