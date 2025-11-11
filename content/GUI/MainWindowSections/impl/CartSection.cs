using GamesShop.content.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using GamesShop.content.models;

namespace GamesShop.content.GUI.MainWindowSections.impl
{
    internal class CartSection : BaseSection
    {
        private string username;
        private List<Game> cartGames;

        public CartSection(string username)
        {
            this.username = username;
            this.cartGames = UserDatabaseManager.GetUserCart(username);
        }

        public override string Title => "Cart";

        public override FrameworkElement Render()
        {
            var scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Background = Brushes.Transparent,
                Padding = new Thickness(20)
            };

            var mainStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Background = Brushes.Transparent
            };

            mainStackPanel.Children.Add(new TextBlock
            {
                Text = $"Корзина пользователя {username}",
                Foreground = Brushes.White,
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 20)
            });

            if (cartGames.Count == 0)
            {
                mainStackPanel.Children.Add(new TextBlock
                {
                    Text = "Корзина пуста",
                    Foreground = Brushes.Gray,
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 50, 0, 0)
                });
            }
            else
            {
                decimal totalPrice = CalculateTotalPrice();
                mainStackPanel.Children.Add(new TextBlock
                {
                    Text = $"Общая стоимость: {totalPrice:F2} $",
                    Foreground = Brushes.White,
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 20)
                });

                var wrapPanel = new WrapPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                foreach (var game in cartGames)
                {
                    var gameCard = CreateGameCard(game);
                    wrapPanel.Children.Add(gameCard);
                }

                mainStackPanel.Children.Add(wrapPanel);

                var checkoutButton = new Button
                {
                    Content = "Оформить заказ",
                    Background = new SolidColorBrush(Color.FromRgb(16, 139, 239)),
                    Foreground = Brushes.White,
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 20, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Cursor = Cursors.Hand
                };

                //checkoutButton.Click += (s, e) => Checkout();
                mainStackPanel.Children.Add(checkoutButton);
            }

            scrollViewer.Content = mainStackPanel;
            return scrollViewer;
        }
        private FrameworkElement CreateGameCard(Game game)
        {
            var cardBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(45, 45, 45)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(80, 80, 80)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Width = 250,
                Height = 320,
                Margin = new Thickness(10),
                Padding = new Thickness(15),
                Cursor = Cursors.Hand
            };

            var cardStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            var imageBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(60, 60, 60)),
                Height = 120,
                CornerRadius = new CornerRadius(6),
                Margin = new Thickness(0, 0, 0, 10)
            };

            if (!string.IsNullOrEmpty(game.Logo))
            {
                var image = new Image
                {
                    Source = new BitmapImage(new Uri(game.Logo, UriKind.RelativeOrAbsolute)),
                    Stretch = Stretch.Uniform,
                    Height = 120
                };
                imageBorder.Child = image;
            }
            else
            {
                imageBorder.Child = new TextBlock
                {
                    Text = "No Image",
                    Foreground = Brushes.Gray,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 12
                };
            }

            cardStackPanel.Children.Add(imageBorder);

            var titleTextBlock = new TextBlock
            {
                Text = game.Title,
                Foreground = Brushes.White,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 5),
                MaxHeight = 40,
                TextTrimming = TextTrimming.CharacterEllipsis
            };
            cardStackPanel.Children.Add(titleTextBlock);

            var genreTextBlock = new TextBlock
            {
                Text = game.Genre,
                Foreground = new SolidColorBrush(Color.FromRgb(180, 180, 180)),
                FontSize = 12,
                FontStyle = FontStyles.Italic,
                Margin = new Thickness(0, 0, 0, 5)
            };
            cardStackPanel.Children.Add(genreTextBlock);

            var priceTextBlock = new TextBlock
            {
                Text = game.GetFormattedPrice(),
                Foreground = new SolidColorBrush(Color.FromRgb(16, 139, 239)),
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            };
            cardStackPanel.Children.Add(priceTextBlock);

            var removeButton = new Button
            {
                Content = "Удалить",
                Background = new SolidColorBrush(Color.FromRgb(200, 60, 60)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                FontSize = 11,
                FontWeight = FontWeights.Bold,
                Cursor = Cursors.Hand
            };

            removeButton.Click += (s, e) =>
            {
                e.Handled = true; // Предотвращаем всплытие события
                UserDatabaseManager.RemoveGameFromCart(username, game.ID);
                


            };

            cardStackPanel.Children.Add(removeButton);

            cardBorder.MouseLeftButtonDown += (s, e) =>
            {
                // Здесь будет переход на страницу игры

            };

            AddHoverEffects(cardBorder);

            cardBorder.Child = cardStackPanel;
            return cardBorder;
        }

        private void AddHoverEffects(Border cardBorder)
        {
            cardBorder.MouseEnter += (s, e) =>
            {
                cardBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(16, 139, 239));
                cardBorder.Background = new SolidColorBrush(Color.FromRgb(50, 50, 50));
            };

            cardBorder.MouseLeave += (s, e) =>
            {
                cardBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(80, 80, 80));
                cardBorder.Background = new SolidColorBrush(Color.FromRgb(45, 45, 45));
            };
        }

        private decimal CalculateTotalPrice()
        {
            decimal total = 0;
            foreach (var game in cartGames)
            {
                total += game.Price;
            }
            return total;
        }
    }
}
