using GamesShop.content.db;
using GamesShop.content.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Azure.Core.HttpHeader;

namespace GamesShop
{
    public partial class MainWindowControl : UserControl
    {
        private string username;
        private int userID;
        private List<Game> games;
        private List<Game> cartGames;

        private Game currentGame;


        public MainWindowControl(string username)
        {
            InitializeComponent();
            this.username = username;
            this.userID = UserDatabaseManager.GetUserId(username);
            InitializeData();
            ShowGamesSection();
        }

        private void InitializeData()
        {
            games = GameDatabseManager.GetAllGames();
            cartGames = UserDatabaseManager.GetUserCart(username);

            //var user = UserDatabaseManager(username);
            //UserBalanceText.Text = user?.Balance.ToString("F2") ?? "0.00";

            WelcomeText.Text = $"Добро пожаловать, {username}!";
            CartTitleText.Text = $"Корзина пользователя {username}";
            ProfileUsernameText.Text = username;
        }

        private void GamesButton_Click(object sender, RoutedEventArgs e)
        {
            ShowGamesSection();
        }

        private void LibraryButton_Click(object sender, RoutedEventArgs e)
        {
            ShowLibrarySection();
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            ShowCartSection();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ShowProfileSection();
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Заказ оформлен!");
        }

        private void ShowGamesSection()
        {
            HideAllSections();
            GamesSection.Visibility = Visibility.Visible;
            UpdateButtonStates(GamesButton);
            RenderGames();
        }

        private void ShowLibrarySection()
        {
            HideAllSections();
            LibrarySection.Visibility = Visibility.Visible;
            UpdateButtonStates(LibraryButton);
        }

        private void ShowCartSection()
        {
            HideAllSections();
            CartSection.Visibility = Visibility.Visible;
            UpdateButtonStates(CartButton);
            RenderCart();
        }

        private void ShowProfileSection()
        {
            HideAllSections();
            ProfileSection.Visibility = Visibility.Visible;
            UpdateButtonStates(null);
        }

        private void HideAllSections()
        {
            GamesSection.Visibility = Visibility.Collapsed;
            LibrarySection.Visibility = Visibility.Collapsed;
            CartSection.Visibility = Visibility.Collapsed;
            ProfileSection.Visibility = Visibility.Collapsed;
            GameDetailsSection.Visibility = Visibility.Collapsed;
        }

        private void UpdateButtonStates(Button activeButton)
        {
            GamesButton.Tag = "";
            LibraryButton.Tag = "";
            CartButton.Tag = "";

            if (activeButton != null)
                activeButton.Tag = "Active";
        }

        private void RenderGames()
        {
            GamesItemsControl.Items.Clear();
            foreach (var game in games)
            {
                var gameCard = CreateGameCard(game, false);
                GamesItemsControl.Items.Add(gameCard);
            }
        }

        private void RenderCart()
        {
            cartGames = UserDatabaseManager.GetUserCart(username);

            if (cartGames.Count == 0)
            {
                CartEmptyText.Visibility = Visibility.Visible;
                TotalPriceText.Visibility = Visibility.Collapsed;
                CheckoutButton.Visibility = Visibility.Collapsed;
                CartItemsControl.Items.Clear();
            }
            else
            {
                CartEmptyText.Visibility = Visibility.Collapsed;
                TotalPriceText.Visibility = Visibility.Visible;
                CheckoutButton.Visibility = Visibility.Visible;

                decimal totalPrice = cartGames.Sum(g => g.Price);
                TotalPriceText.Text = $"Общая стоимость: {totalPrice:F2} $";

                CartItemsControl.Items.Clear();
                foreach (var game in cartGames)
                {
                    var gameCard = CreateGameCard(game, true);
                    CartItemsControl.Items.Add(gameCard);
                }
            }
        }

        private FrameworkElement CreateGameCard(Game game, bool isCartItem)
        {
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(45, 45, 45)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(80, 80, 80)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Width = isCartItem ? 250 : 280,
                Height = isCartItem ? 320 : 400,
                Margin = new Thickness(10),
                Padding = new Thickness(15),
                Cursor = Cursors.Hand
            };

            var stackPanel = new StackPanel();

            var imageBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(60, 60, 60)),
                Height = isCartItem ? 120 : 160,
                CornerRadius = new CornerRadius(6),
                Margin = new Thickness(0, 0, 0, 10)
            };

            string fullPath = "content/Images/" + game.Logo;

            var image = new Image
            {
                Source = new BitmapImage(new Uri(fullPath, UriKind.RelativeOrAbsolute)),
                Stretch = Stretch.Uniform,
                Height = isCartItem ? 120 : 160
            };
            imageBorder.Child = image;

            image.ImageFailed += (s, e) =>
            {
                image.Source = new BitmapImage(new Uri("/content/Images/default_game.png", UriKind.Relative));
            };

            imageBorder.Child = image;
            stackPanel.Children.Add(imageBorder);

            var titleText = new TextBlock
            {
                Text = game.Title,
                Foreground = Brushes.White,
                FontSize = isCartItem ? 16 : 18,
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 8),
                MaxHeight = 40,
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            stackPanel.Children.Add(titleText);
            string genres = "";
            foreach (var genre in GameDatabseManager.GetGameGenres(game.ID))
            {
                genres += genre.Name + ", ";
            }

            if (!string.IsNullOrEmpty(genres))
            {
                genres = genres.TrimEnd(',', ' ');
            }
            var genreText = new TextBlock
            {
                Text = genres,
                Foreground = new SolidColorBrush(Color.FromRgb(180, 180, 180)),
                FontSize = 12,
                FontStyle = FontStyles.Italic,
                Margin = new Thickness(0, 0, 0, 8)
            };
            stackPanel.Children.Add(genreText);

            if (!isCartItem)
            {
                var descriptionText = new TextBlock
                {
                    Text = TruncateDescription(game.Description, 120),
                    Foreground = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
                    FontSize = 12,
                    TextWrapping = TextWrapping.Wrap,
                    Height = 60,
                    Margin = new Thickness(0, 0, 0, 15),
                    TextTrimming = TextTrimming.CharacterEllipsis
                };
                stackPanel.Children.Add(descriptionText);
            }

            var priceText = new TextBlock
            {
                Text = $"{game.Price:F2} $",
                Foreground = new SolidColorBrush(Color.FromRgb(16, 139, 239)),
                FontSize = isCartItem ? 14 : 16,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, isCartItem ? 10 : 15)
            };
            stackPanel.Children.Add(priceText);
            bool isInCart = UserDatabaseManager.IsGameInCart(username, game.ID);

            if (!isCartItem)
            {
                Button cartButton;

                if (isInCart)
                {
                    cartButton = new Button
                    {
                        Content = "✓ Уже в корзине",
                        Background = new SolidColorBrush(Color.FromRgb(100, 100, 100)),
                        Foreground = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
                        BorderThickness = new Thickness(0),
                        FontSize = 12,
                        FontWeight = FontWeights.Bold,
                        Cursor = Cursors.Arrow,
                        IsEnabled = false,
                        Tag = game.ID
                    };
                }
                else
                {
                    cartButton = new Button
                    {
                        Content = "В корзину",
                        Background = new SolidColorBrush(Color.FromRgb(16, 139, 239)),
                        Foreground = Brushes.White,
                        BorderThickness = new Thickness(0),
                        FontSize = 12,
                        FontWeight = FontWeights.Bold,
                        Cursor = Cursors.Hand,
                        Tag = game.ID
                    };

                    cartButton.MouseEnter += (s, e) =>
                    {
                        cartButton.Background = new SolidColorBrush(Color.FromRgb(20, 120, 220));
                    };
                    cartButton.MouseLeave += (s, e) =>
                    {
                        cartButton.Background = new SolidColorBrush(Color.FromRgb(16, 139, 239));
                    };

                    cartButton.Click += (s, e) =>
                    {
                        UserDatabaseManager.AddGameToCart(username, game.ID);

                        UpdateButtonToInCart(cartButton);

                        e.Handled = true;
                    };
                }

                stackPanel.Children.Add(cartButton);
            }
            else
            {
                var removeButton = new Button
                {
                    Content = "Удалить",
                    Background = new SolidColorBrush(Color.FromRgb(200, 60, 60)),
                    Foreground = Brushes.White,
                    BorderThickness = new Thickness(0),
                    FontSize = 11,
                    FontWeight = FontWeights.Bold,
                    Cursor = Cursors.Hand,
                    Tag = game.ID
                };

                removeButton.MouseEnter += (s, e) =>
                {
                    removeButton.Background = new SolidColorBrush(Color.FromRgb(220, 80, 80));
                };
                removeButton.MouseLeave += (s, e) =>
                {
                    removeButton.Background = new SolidColorBrush(Color.FromRgb(200, 60, 60));
                };

                removeButton.Click += (s, e) =>
                {
                    UserDatabaseManager.RemoveGameFromCart(username, game.ID);
                    ShowCartSection();
                    e.Handled = true;
                };

                stackPanel.Children.Add(removeButton);
            }

            border.MouseEnter += (s, e) =>
            {
                if (!isCartItem || isInCart)
                {
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(16, 139, 239));
                    border.Background = new SolidColorBrush(Color.FromRgb(50, 50, 50));
                }
            };

            border.MouseLeave += (s, e) =>
            {
                border.BorderBrush = new SolidColorBrush(Color.FromRgb(80, 80, 80));
                border.Background = new SolidColorBrush(Color.FromRgb(45, 45, 45));
            };

            border.MouseLeftButtonDown += (s, e) =>
            {
                ShowGameDetails(game);
                e.Handled = true;
            };

            border.Child = stackPanel;
            return border;
        }

        private void ShowGameDetails(Game game)
        {
            currentGame = game;
            HideAllSections();
            GameDetailsSection.Visibility = Visibility.Visible;

            SetNavigationEnabled(false);

            UpdateButtonStates(null);
            LoadGameDetails(game);
        }

        private void SetNavigationEnabled(bool enabled)
        {
            var navButtons = new[] { GamesButton, LibraryButton, CartButton, ProfileButtonControl };

            foreach (var button in navButtons)
            {
                if (button != null)
                {
                    button.IsEnabled = enabled;
                    button.Opacity = enabled ? 1.0 : 0.5;
                }
            }

            if (BackButton != null)
            {
                BackButton.Visibility = enabled ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        private void LoadGameDetails(Game game)
        {
            GameDetailsTitle.Text = game.Title;

            string genres = "";
            foreach (var genre in GameDatabseManager.GetGameGenres(game.ID))
            {
                genres += genre.Name + ", ";
            }

            if (!string.IsNullOrEmpty(genres))
            {
                genres = genres.TrimEnd(',', ' ');
            }
            GameDetailsGenre.Text = genres;
            GameDetailsPrice.Text = $"{game.Price:F2} $";
            GameDetailsDescription.Text = game.Description;
            GameDetailsRating.Text = GetRatingStars(game.Rating);

            try
            {
                string fullPath = "content/Images/" + game.Logo;

                GameDetailsImage.Source = new BitmapImage(new Uri(fullPath, UriKind.RelativeOrAbsolute));
            }
            catch
            {
                GameDetailsImage.Source = new BitmapImage(new Uri("/content/Images/default_game.png", UriKind.Relative));
            }

            GameDetailsReleaseDate.Text = game.ReleaseDate.ToString("dd.MM.yyyy") ?? "Неизвестно";

            LoadGameFeatures(game);
            LoadGameReviews(game.ID);
        }

        private void LoadGameFeatures(Game game)
        {
            GameDetailsFeatures.Children.Clear();

            string dev = "";
            foreach (var devs in GameDatabseManager.GetGameDevelopers(game.ID))
            {
                dev += devs.Name + ", ";
            }

            if (!string.IsNullOrEmpty(dev))
            {
                dev = dev.TrimEnd(',', ' ');
            }

            string lang = "";
            foreach (var langs in GameDatabseManager.GetGameLanguages(game.ID))
            {
                lang += langs.LanguageName + ", ";
            }

            if (!string.IsNullOrEmpty(dev))
            {
                lang = lang.TrimEnd(',', ' ');
            }


            var features = new[]
            {
                $"Разработчик: {dev}",
                $"Платформа: {game.Platform ?? "PC"}",
                $"Язык: {lang}",
                $"Возрастной рейтинг: {game.AgeRating.ToString() + "+" ?? "18+"}"
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
            GameDetailsReviewsList.Children.Clear();

            var reviews = GameDatabseManager.GetGameReviews(gameId);
            GameDetailsReviewsCount.Text = $"({reviews.Count})";

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
                Margin = new Thickness(0, 0, 0, 10)
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

            stackPanel.Children.Add(new TextBlock
            {
                Text = review.ReviewText,
                Foreground = new SolidColorBrush(Color.FromRgb(220, 220, 220)),
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 5, 0, 0)
            });

            border.Child = stackPanel;
            return border;
        }

        private void BuyGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentGame != null)
            {
                MessageBox.Show($"Покупка игры \"{currentGame.Title}\" за {currentGame.Price:F2} $\n\n" +
                               "Здесь будет интеграция с платежной системой",
                               "Покупка игры", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddToCartDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentGame != null)
            {
                UserDatabaseManager.AddGameToCart(username, currentGame.ID);
                
                AddToCartDetailsButton.Content = "✓ В корзине";
                AddToCartDetailsButton.IsEnabled = false;
                AddToCartDetailsButton.Background = new SolidColorBrush(Color.FromRgb(100, 100, 100));
            }
        }

        private void AddReviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentGame == null)
            {
                MessageBox.Show("Не выбрана игра для отзыва", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    MessageBox.Show("Пожалуйста, поставьте оценку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(reviewTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, напишите текст отзыва", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (reviewTextBox.Text.Length < 10)
                {
                    MessageBox.Show("Отзыв должен содержать минимум 10 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (UserDatabaseManager.UserHasReviewForGame(username, currentGame.ID))
                {
                    MessageBox.Show("Отзыв уже оставлен!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newReview = new Review
                {
                    GameID = currentGame.ID,
                    UserID = this.userID,
                    ReviewText = reviewTextBox.Text.Trim(),
                    Rating = selectedRating,
                    ReviewDate = DateTime.Now
                };

                bool success = GameDatabseManager.AddReview(newReview);

                if (success)
                {
                    MessageBox.Show("Отзыв успешно опубликован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    reviewDialog.Close();

                    LoadGameReviews(currentGame.ID);

                    UpdateGameRating();
                }
                else
                {
                    MessageBox.Show("Ошибка при добавлении отзыва", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };

            buttonsPanel.Children.Add(cancelButton);
            buttonsPanel.Children.Add(submitButton);
            mainStackPanel.Children.Add(buttonsPanel);

            reviewDialog.Content = mainStackPanel;
            reviewDialog.ShowDialog();
        }

        private void UpdateGameRating()
        {
            var reviews = GameDatabseManager.GetGameReviews(currentGame.ID);
            if (reviews.Count > 0)
            {
                double averageRating = reviews.Average(r => r.Rating);
                GameDatabseManager.UpdateGameRating(currentGame.ID, averageRating);

                GameDetailsRating.Text = GetRatingStars(averageRating);
            }
        }

        private string TruncateDescription(string description, int maxLength)
        {
            if (string.IsNullOrEmpty(description) || description.Length <= maxLength)
                return description;

            return description.Substring(0, maxLength) + "...";
        }

        private string GetRatingStars(double rating)
        {
            int fullStars = (int)rating;
            bool hasHalfStar = (rating - fullStars) >= 0.5;

            string stars = new string('★', fullStars);
            if (hasHalfStar) stars += "⯪";

            return stars;
        }

        private void UpdateButtonToInCart(Button button)
        {
            button.Content = "✓ Уже в корзине";
            button.Background = new SolidColorBrush(Color.FromRgb(137, 233, 51));
            button.Foreground = new SolidColorBrush(Color.FromRgb(137, 233, 51));
            button.Cursor = Cursors.Arrow;
            button.IsEnabled = false;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnToPreviousView();
        }
        private void ReturnToPreviousView()
        {
            SetNavigationEnabled(true);

            GameDetailsSection.Visibility = Visibility.Collapsed;

            GamesSection.Visibility = Visibility.Visible;
            UpdateButtonStates(GamesButton);

            currentGame = null;
        }
    }

}
