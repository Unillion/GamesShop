using GamesShop.content.db;
using GamesShop.content.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace GamesShop.content.GUI.GUI_services
{
    public class GameDetailsService
    {
        private readonly string username;
        private readonly int userId;
        private Game currentGame;
        public TextBlock GameDetailsTitle { get; set; }
        public TextBlock GameDetailsGenre { get; set; }
        public TextBlock GameDetailsPrice { get; set; }
        public TextBlock GameDetailsDescription { get; set; }
        public TextBlock GameDetailsRating { get; set; }
        public Image GameDetailsImage { get; set; }
        public TextBlock GameDetailsReleaseDate { get; set; }
        public StackPanel GameDetailsFeatures { get; set; }
        public StackPanel GameDetailsReviewsList { get; set; }
        public TextBlock GameDetailsReviewsCount { get; set; }
        public Button AddToCartDetailsButton { get; set; }

        public GameDetailsService(string username, int userId)
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
                GameDetailsPrice.Text = $"{game.Price:F2} $";
                GameDetailsDescription.Text = game.Description;
                GameDetailsRating.Text = GetRatingStars(game.Rating);
                GameDetailsReleaseDate.Text = game.ReleaseDate.ToString("dd.MM.yyyy") ?? "Неизвестно";
                GameDetailsReviewsCount.Text = $"({GetReviewsCount(game.ID)})";

                LoadGameImage(game);
                LoadGameFeatures(game);
                LoadGameReviews(game.ID);
                UpdateAddToCartButton();
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

        private void LoadGameFeatures(Game game)
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

        private void LoadGameReviews(int gameId)
        {
            if (GameDetailsReviewsList == null) return;

            GameDetailsReviewsList.Children.Clear();

            var reviews = GameDatabseManager.GetGameReviews(gameId);

            if (reviews.Count == 0)
            {
                var noReviewsText = new TextBlock
                {
                    Text = "Пока нет отзывов",
                    Foreground = new SolidColorBrush(Color.FromRgb(180, 180, 180)),
                    FontSize = 14,
                    FontStyle = FontStyles.Italic,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 20, 0, 0)
                };
                GameDetailsReviewsList.Children.Add(noReviewsText);
                return;
            }

            foreach (var review in reviews)
            {
                var reviewPanel = CreateReviewPanel(review);
                GameDetailsReviewsList.Children.Add(reviewPanel);
            }
        }

        private FrameworkElement CreateReviewPanel(Review review)
        {
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(40, 40, 40)),
                Padding = new Thickness(15),
                Margin = new Thickness(0, 0, 0, 10),
                CornerRadius = new CornerRadius(8)
            };

            var stackPanel = new StackPanel();

            var headerPanel = new StackPanel { Orientation = Orientation.Horizontal };
            headerPanel.Children.Add(new TextBlock
            {
                Text = review.User.Username,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                FontSize = 14
            });

            headerPanel.Children.Add(new TextBlock
            {
                Text = $" - {review.ReviewDate:dd.MM.yyyy}",
                Foreground = new SolidColorBrush(Color.FromRgb(150, 150, 150)),
                FontSize = 12,
                Margin = new Thickness(10, 0, 0, 0)
            });

            stackPanel.Children.Add(headerPanel);

            var ratingText = new TextBlock
            {
                Text = GetRatingStars(review.Rating),
                Foreground = Brushes.Gold,
                FontSize = 12,
                Margin = new Thickness(0, 2, 0, 0)
            };
            stackPanel.Children.Add(ratingText);

            stackPanel.Children.Add(new TextBlock
            {
                Text = review.ReviewText,
                Foreground = new SolidColorBrush(Color.FromRgb(220, 220, 220)),
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 8, 0, 0)
            });

            var likesDislikesPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 12, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            var likeButton = new Button
            {
                Content = $"👍 {review.Likes}",
                Background = new SolidColorBrush(Colors.Transparent),
                Foreground = new SolidColorBrush(Color.FromRgb(100, 200, 100)),
                BorderThickness = new Thickness(0),
                FontSize = 11,
                Padding = new Thickness(8, 4, 8, 4),
                Cursor = Cursors.Hand,
                Tag = review.Likes
            };

            var dislikeButton = new Button
            {
                Content = $"👎 {review.Dislikes}",
                Background = new SolidColorBrush(Colors.Transparent),
                Foreground = new SolidColorBrush(Color.FromRgb(200, 100, 100)),
                BorderThickness = new Thickness(0),
                FontSize = 11,
                Padding = new Thickness(8, 4, 8, 4),
                Cursor = Cursors.Hand,
                Tag = review.Dislikes,
                Margin = new Thickness(10, 0, 0, 0)
            };

            likeButton.MouseEnter += (s, e) =>
            {
                likeButton.Background = new SolidColorBrush(Color.FromRgb(60, 60, 60));
            };
            likeButton.MouseLeave += (s, e) =>
            {
                likeButton.Background = new SolidColorBrush(Colors.Transparent);
            };

            dislikeButton.MouseEnter += (s, e) =>
            {
                dislikeButton.Background = new SolidColorBrush(Color.FromRgb(60, 60, 60));
            };
            dislikeButton.MouseLeave += (s, e) =>
            {
                dislikeButton.Background = new SolidColorBrush(Colors.Transparent);
            };

            likeButton.Click += (s, e) =>
            {
                InsertLike(review.ReviewID, likeButton, dislikeButton);
            };

            dislikeButton.Click += (s, e) =>
            {
                InsertDislike(review.ReviewID, likeButton, dislikeButton);
            };

            likesDislikesPanel.Children.Add(likeButton);
            likesDislikesPanel.Children.Add(dislikeButton);
            stackPanel.Children.Add(likesDislikesPanel);

            border.Child = stackPanel;
            return border;
        }

        public void AddToCart()
        {
            if (currentGame != null)
            {
                try
                {
                    UserDatabaseManager.AddGameToCart(username, currentGame.ID);
                    UpdateAddToCartButton();
                    MessageBox.Show($"Игра \"{currentGame.Title}\" добавлена в корзину!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении в корзину: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void AddReview()
        {
            if (currentGame == null)
            {
                MessageBox.Show("Не выбрана игра для отзыва", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (UserDatabaseManager.UserHasReviewForGame(username, currentGame.ID))
            {
                MessageBox.Show("Вы уже оставили отзыв на эту игру!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var reviewDialog = new Window
            {
                Title = "Добавить отзыв",
                Width = 500,
                Height = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = Application.Current.MainWindow,
                ResizeMode = ResizeMode.NoResize,
                Background = new SolidColorBrush(Color.FromRgb(45, 45, 45))
            };

            var mainStackPanel = new StackPanel
            {
                Margin = new Thickness(20)
            };

            var titleText = new TextBlock
            {
                Text = $"Отзыв на игру: {currentGame.Title}",
                Foreground = Brushes.White,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 20),
                TextAlignment = TextAlignment.Center
            };
            mainStackPanel.Children.Add(titleText);

            var ratingPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 15)
            };

            ratingPanel.Children.Add(new TextBlock
            {
                Text = "Оценка:",
                Foreground = Brushes.White,
                FontSize = 14,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 0)
            });

            var starsPanel = new StackPanel { Orientation = Orientation.Horizontal };
            int selectedRating = 0;

            for (int i = 1; i <= 5; i++)
            {
                var starButton = new Button
                {
                    Content = "☆",
                    Foreground = new SolidColorBrush(Color.FromRgb(150, 150, 150)),
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0),
                    FontSize = 20,
                    Tag = i,
                    Margin = new Thickness(5, 0, 5, 0),
                    Padding = new Thickness(5),
                    Cursor = Cursors.Hand
                };

                starButton.Click += (s, args) =>
                {
                    selectedRating = (int)((Button)s).Tag;
                    foreach (Button star in starsPanel.Children)
                    {
                        int starValue = (int)star.Tag;
                        star.Content = starValue <= selectedRating ? "★" : "☆";
                        star.Foreground = starValue <= selectedRating ? Brushes.Gold : new SolidColorBrush(Color.FromRgb(150, 150, 150));
                    }
                };

                starsPanel.Children.Add(starButton);
            }

            ratingPanel.Children.Add(starsPanel);
            mainStackPanel.Children.Add(ratingPanel);

            var reviewTextLabel = new TextBlock
            {
                Text = "Текст отзыва:",
                Foreground = Brushes.White,
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 5)
            };
            mainStackPanel.Children.Add(reviewTextLabel);

            var reviewTextBox = new TextBox
            {
                Height = 150,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Background = new SolidColorBrush(Color.FromRgb(60, 60, 60)),
                Foreground = Brushes.White,
                BorderBrush = new SolidColorBrush(Color.FromRgb(100, 100, 100)),
                BorderThickness = new Thickness(1),
                FontSize = 12,
                Padding = new Thickness(10)
            };
            mainStackPanel.Children.Add(reviewTextBox);

            var buttonsPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 20, 0, 0)
            };

            var cancelButton = new Button
            {
                Content = "Отмена",
                Width = 100,
                Height = 30,
                Background = new SolidColorBrush(Color.FromRgb(100, 100, 100)),
                Foreground = Brushes.White,
                Margin = new Thickness(0, 0, 10, 0),
                Cursor = Cursors.Hand
            };
            cancelButton.Click += (s, args) => reviewDialog.Close();

            var submitButton = new Button
            {
                Content = "Опубликовать",
                Width = 120,
                Height = 30,
                Background = new SolidColorBrush(Color.FromRgb(70, 130, 180)),
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                Cursor = Cursors.Hand
            };
            submitButton.Click += (s, args) =>
            {
                if (selectedRating == 0)
                {
                    MessageBox.Show("Пожалуйста, поставьте оценку", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(reviewTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, напишите текст отзыва", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (reviewTextBox.Text.Length < 10)
                {
                    MessageBox.Show("Отзыв должен содержать минимум 10 символов", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newReview = new Review
                {
                    GameID = currentGame.ID,
                    UserID = userId,
                    ReviewText = reviewTextBox.Text.Trim(),
                    Rating = selectedRating,
                    ReviewDate = DateTime.Now
                };

                bool success = GameDatabseManager.AddReview(newReview);

                if (success)
                {
                    MessageBox.Show("Отзыв успешно опубликован!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    reviewDialog.Close();

                    LoadGameReviews(currentGame.ID);
                    UpdateGameRating();
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении отзыва", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };

            buttonsPanel.Children.Add(cancelButton);
            buttonsPanel.Children.Add(submitButton);
            mainStackPanel.Children.Add(buttonsPanel);

            reviewDialog.Content = mainStackPanel;
            reviewDialog.ShowDialog();
        }

        public void UpdateAddToCartButton()
        {
            if (AddToCartDetailsButton != null && currentGame != null)
            {
                bool isInCart = UserDatabaseManager.IsGameInCart(username, currentGame.ID);

                if (isInCart)
                {
                    AddToCartDetailsButton.Content = "✓ В корзине";
                    AddToCartDetailsButton.IsEnabled = false;
                    AddToCartDetailsButton.Background = new SolidColorBrush(Color.FromRgb(100, 100, 100));
                }
                else
                {
                    AddToCartDetailsButton.Content = "В корзину";
                    AddToCartDetailsButton.IsEnabled = true;
                    AddToCartDetailsButton.Background = new SolidColorBrush(Color.FromRgb(16, 139, 239));
                }
            }
        }

        private void UpdateGameRating()
        {
            var reviews = GameDatabseManager.GetGameReviews(currentGame.ID);
            if (reviews.Count > 0)
            {
                double averageRating = reviews.Average(r => r.Rating);
                GameDatabseManager.UpdateGameRating(currentGame.ID, averageRating);

                if (GameDetailsRating != null)
                {
                    GameDetailsRating.Text = GetRatingStars(averageRating);
                }
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

        private int GetReviewsCount(int gameId)
        {
            var reviews = GameDatabseManager.GetGameReviews(gameId);
            return reviews.Count;
        }

        private void InsertLike(int reviewID, Button likeButton, Button dislikeButton)
        {
            try
            {
                int currentLikes = (int)likeButton.Tag;
                int currentDislikes = (int)dislikeButton.Tag;

                if (dislikeButton.Foreground.ToString() == "#FFC86464")
                {
                    GameDatabseManager.RemoveDislikeFromReview(reviewID);
                    dislikeButton.Foreground = new SolidColorBrush(Color.FromRgb(200, 100, 100));
                    currentDislikes--;
                    dislikeButton.Tag = currentDislikes;
                    dislikeButton.Content = $"👎 {currentDislikes}";
                }

                GameDatabseManager.AddLikeToReview(reviewID);
                likeButton.Foreground = new SolidColorBrush(Color.FromRgb(100, 255, 100));
                currentLikes++;
                likeButton.Tag = currentLikes;
                likeButton.Content = $"👍 {currentLikes}";

                if (currentGame != null)
                {
                    LoadGameReviews(currentGame.ID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении лайка: {ex.Message}");
            }
        }

        private void InsertDislike(int reviewID, Button likeButton, Button dislikeButton)
        {
            try
            {
                int currentLikes = (int)likeButton.Tag;
                int currentDislikes = (int)dislikeButton.Tag;

                if (likeButton.Foreground.ToString() == "#FF64FF64")
                {
                    GameDatabseManager.RemoveLikeFromReview(reviewID);
                    likeButton.Foreground = new SolidColorBrush(Color.FromRgb(100, 200, 100));
                    currentLikes--;
                    likeButton.Tag = currentLikes;
                    likeButton.Content = $"👍 {currentLikes}";
                }

                GameDatabseManager.AddDislikeToReview(reviewID);
                dislikeButton.Foreground = new SolidColorBrush(Color.FromRgb(255, 100, 100));
                currentDislikes++;
                dislikeButton.Tag = currentDislikes;
                dislikeButton.Content = $"👎 {currentDislikes}";

                if (currentGame != null)
                {
                    LoadGameReviews(currentGame.ID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении дизлайка: {ex.Message}");
            }
        }

        private string GetRatingStars(double rating)
        {
            int fullStars = (int)rating;
            bool hasHalfStar = (rating - fullStars) >= 0.5;

            string stars = new string('★', fullStars);
            if (hasHalfStar) stars += "⯪";

            return stars;
        }
    }
}
