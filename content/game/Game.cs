using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.content.game
{
    public class Game
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Rating { get; set; }
        public string Logo { get; set; }

        public Game() { }

        public Game(int id, string title, string description, string genre, decimal price, DateTime releaseDate, int rating, string logo)
        {
            ID = id;
            Title = title;
            Description = description;
            Genre = genre;
            Price = price;
            ReleaseDate = releaseDate;
            Rating = rating;
            Logo = "content/Images/"+logo;
        }

        public string GetFormattedPrice()
        {
            return Price.ToString() + " $";
        }

        public string GetFormattedReleaseDate()
        {
            return ReleaseDate.ToString("dd.MM.yyyy");
        }

        public string GetRatingStars()
        {
            return new string('★', Rating) + new string('☆', 5 - Rating);
        }

        public override string ToString()
        {
            return $"{Title} ({Genre}) - {GetFormattedPrice()}";
        }
    }
}
