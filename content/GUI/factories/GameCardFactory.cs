using GamesShop.content.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using GamesShop.content.db;

namespace GamesShop.content.GUI.factories
{
    public class GameCardFactory
    {
        public static FrameworkElement CreateGameCard(Game game, bool isCartItem, bool isLibraryItem, string username,
            Action<Game> onGameClick, Action<int> onCartAction)
        {
            var border = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(45, 45, 45)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(80, 80, 80)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Width = isCartItem | isLibraryItem ? 250 : 280,
                Height = isCartItem | isLibraryItem ? 300 : 400,
                Margin = new Thickness(10),
                Padding = new Thickness(15),
                Cursor = Cursors.Hand
            };

            var stackPanel = new StackPanel();

            AddImage(stackPanel, game, isCartItem);
            AddTitle(stackPanel, game, isCartItem);
            AddGenres(stackPanel, game);

            if (!isCartItem)
            {
                AddDescription(stackPanel, game);
            }

            AddPrice(stackPanel, game, isCartItem, isLibraryItem);
            AddActionButton(stackPanel, game, isCartItem, isLibraryItem, username, onCartAction);

            AddEventHandlers(border, game, isCartItem, username, onGameClick);

            border.Child = stackPanel;
            return border;
        }

        private static void AddImage(StackPanel stackPanel, Game game, bool isCartItem)
        {
            var imageBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(60, 60, 60)),
                Height = isCartItem ? 120 : 160,
                CornerRadius = new CornerRadius(6),
                Margin = new Thickness(0, 0, 0, 10)
            };

            var image = new Image
            {
                Source = LoadImage(game.Logo),
                Stretch = Stretch.Uniform,
                Height = isCartItem ? 120 : 160
            };

            imageBorder.Child = image;
            stackPanel.Children.Add(imageBorder);
        }

        private static void AddTitle(StackPanel stackPanel, Game game, bool isCartItem)
        {
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
        }

        private static void AddGenres(StackPanel stackPanel, Game game)
        {
            string genres = GetGameGenres(game.ID);

            if (!string.IsNullOrEmpty(genres))
            {
                var genreText = new TextBlock
                {
                    Text = genres,
                    Foreground = new SolidColorBrush(Color.FromRgb(180, 180, 180)),
                    FontSize = 12,
                    FontStyle = FontStyles.Italic,
                    Margin = new Thickness(0, 0, 0, 8)
                };
                stackPanel.Children.Add(genreText);
            }
        }

        private static void AddDescription(StackPanel stackPanel, Game game)
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

        private static void AddPrice(StackPanel stackPanel, Game game, bool isCartItem, bool isLibItem)
        {
            if (isLibItem) return;
            var priceText = new TextBlock
            {
                Text = $"{game.Price:F2} ",
                Foreground = new SolidColorBrush(Color.FromRgb(16, 139, 239)),
                FontSize = isCartItem ? 14 : 16,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, isCartItem ? 10 : 15)
            };
            stackPanel.Children.Add(priceText);
        }

        private static void AddActionButton(StackPanel stackPanel, Game game, bool isCartItem, bool isLibItem,
            string username, Action<int> onCartAction)
        {
            if (isLibItem) return;

            bool isGamePurchased = UserDatabaseManager.IsGamePurchased(username, game.ID);

            if (isGamePurchased)
            {
                var purchasedLabel = CreatePurchasedLabel();
                stackPanel.Children.Add(purchasedLabel);
            }
            else if (!isCartItem)
            {
                bool isInCart = UserDatabaseManager.IsGameInCart(username, game.ID);
                var cartButton = CreateCartButton(game.ID, isInCart, onCartAction);
                stackPanel.Children.Add(cartButton);
            }
            else
            {
                var removeButton = CreateRemoveButton(game.ID, onCartAction);
                stackPanel.Children.Add(removeButton);
            }
        }

        private static TextBlock CreatePurchasedLabel()
        {
            return new TextBlock
            {
                Text = "Куплено",
                Foreground = new SolidColorBrush(Color.FromRgb(100, 200, 100)),
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 5, 0, 0),
                Padding = new Thickness(10, 5, 10, 5),
                Background = new SolidColorBrush(Color.FromRgb(60, 100, 60)),
            };
        }

        private static Button CreateCartButton(int gameId, bool isInCart, Action<int> onCartAction)
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
                    Margin = new Thickness(0, 5, 0, 0),
                    Padding = new Thickness(10, 5, 10, 5),
                    FontWeight = FontWeights.Bold,
                    Cursor = Cursors.Arrow,
                    IsEnabled = false,
                    Tag = gameId
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
                    Margin = new Thickness(0, 5, 0, 0),
                    Padding = new Thickness(10, 5, 10, 5),
                    FontWeight = FontWeights.Bold,
                    Cursor = Cursors.Hand,
                    Tag = gameId
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
                    onCartAction?.Invoke(gameId);
                    UpdateButtonToInCart(cartButton);
                    e.Handled = true;
                };
            }

            return cartButton;
        }

        private static Button CreateRemoveButton(int gameId, Action<int> onCartAction)
        {
            var removeButton = new Button
            {
                Content = "Удалить",
                Background = new SolidColorBrush(Color.FromRgb(200, 60, 60)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                FontSize = 11,
                Margin = new Thickness(0, 5, 0, 0),
                Padding = new Thickness(10, 5, 10, 5),
                FontWeight = FontWeights.Bold,
                Cursor = Cursors.Hand,
                Tag = gameId
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
                onCartAction?.Invoke(gameId);
                e.Handled = true;
            };

            return removeButton;
        }

        private static void AddEventHandlers(Border border, Game game, bool isCartItem,
            string username, Action<Game> onGameClick)
        {
            bool isInCart = UserDatabaseManager.IsGameInCart(username, game.ID);

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
                onGameClick?.Invoke(game);
                e.Handled = true;
            };
        }

        private static ImageSource LoadImage(string logoPath)
        {
            try
            {
                string fullPath = "content/Images/" + logoPath;
                return new BitmapImage(new Uri(fullPath, UriKind.RelativeOrAbsolute));
            }
            catch
            {
                return new BitmapImage(new Uri("/content/Images/default_game.png", UriKind.Relative));
            }
        }

        private static string GetGameGenres(int gameId)
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

        private static string TruncateDescription(string description, int maxLength)
        {
            if (string.IsNullOrEmpty(description) || description.Length <= maxLength)
                return description;

            return description.Substring(0, maxLength) + "...";
        }

        private static void UpdateButtonToInCart(Button button)
        {
            button.Content = "✓ Уже в корзине";
            button.Background = new SolidColorBrush(Color.FromRgb(100, 100, 100));
            button.Foreground = new SolidColorBrush(Color.FromRgb(200, 200, 200));
            button.Cursor = Cursors.Arrow;
            button.IsEnabled = false;
        }
    }
}
