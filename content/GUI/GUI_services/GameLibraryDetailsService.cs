using GamesShop.content.db;
using GamesShop.content.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GamesShop.content.GUI.GUI_services
{
    public class GameLibraryDetailsService
    {
        public TextBlock GameDetailsTitle { get; set; }
        public TextBlock GameDetailsGenre { get; set; }
        public TextBlock GameDetailsDescription { get; set; }
        public Image GameDetailsImage { get; set; }
        public TextBlock GameDetailsReleaseDate { get; set; }
        public TextBlock GameDetailsStatistics { get; set; }
        public StackPanel GameDetailsFeatures { get; set; }

        public TextBlock GameLibraryAcivationKey { get; set; }

        private readonly string username;
        private readonly int userId;
        private Game currentGame;

        public GameLibraryDetailsService(string username, int userId)
        {
            this.username = username;
            this.userId = userId;
        }

        public void LoadGameDetails(Game game)
        {
            currentGame = game;

            if (GameDetailsTitle != null)
            {
                GameDetailsTitle.Text = game.Title;
                GameDetailsGenre.Text = GetGameGenres(game.ID);
                GameDetailsDescription.Text = game.Description;
                GameDetailsReleaseDate.Text = game.ReleaseDate.ToString("dd.MM.yyyy") ?? "Неизвестно";
                GameLibraryAcivationKey.Text = "PLACEHOLDER";
                GameDetailsStatistics.Text = "PLACEHOLDER";

                LoadGameImage(game);
                LoadGameFeatures(game);
            }
        }

        private void LoadGameImage(Game game)
        {
            try
            {
                string fullPath = "content/Images/" + game.Logo;
                GameDetailsImage.Source = new BitmapImage(new Uri(fullPath, UriKind.RelativeOrAbsolute));
            }
            catch
            {
                GameDetailsImage.Source = new BitmapImage(new Uri("/content/Images/default_game.png", UriKind.Relative));
            }
        }

        public void LoadGameFeatures(Game game)
        {
            if (GameDetailsFeatures == null) return;

            GameDetailsFeatures.Children.Clear();

            string developers = GetGameDevelopers(game.ID);
            string languages = GetGameLanguages(game.ID);

            var features = new[]
            {
                $"Разработчик: {developers}",
                $"Платформа: {game.Platform ?? "PC"}",
                $"Язык: {languages}",
                $"Возрастной рейтинг: {(game.AgeRating > 0 ? game.AgeRating.ToString() + "+" : "18+")}"
            };

            foreach (var feature in features)
            {
                var featureText = new TextBlock
                {
                    Text = $"• {feature}",
                    Foreground = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
                    FontSize = 12,
                    Margin = new Thickness(0, 2, 0, 2)
                };
                GameDetailsFeatures.Children.Add(featureText);
            }
        }
        private string GetGameGenres(int gameId)
        {
            var genres = GameDatabseManager.GetGameGenres(gameId);
            string genreText = "";

            foreach (var genre in genres)
            {
                genreText += genre.Name + ", ";
            }

            if (!string.IsNullOrEmpty(genreText))
            {
                genreText = genreText.TrimEnd(',', ' ');
            }

            return genreText;
        }
        private string GetGameDevelopers(int gameId)
        {
            var developers = GameDatabseManager.GetGameDevelopers(gameId);
            string developerText = "";

            foreach (var developer in developers)
            {
                developerText += developer.Name + ", ";
            }

            if (!string.IsNullOrEmpty(developerText))
            {
                developerText = developerText.TrimEnd(',', ' ');
            }

            return developerText;
        }

        private string GetGameLanguages(int gameId)
        {
            var languages = GameDatabseManager.GetGameLanguages(gameId);
            string languageText = "";

            foreach (var language in languages)
            {
                languageText += language.LanguageName + ", ";
            }

            if (!string.IsNullOrEmpty(languageText))
            {
                languageText = languageText.TrimEnd(',', ' ');
            }

            return languageText;
        }
    }
}
