using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using GamesShop.content.game;
using GamesShop.content.db;
using System.Collections.Generic;
using System;

namespace GamesShop.content.GUI.MainWindowSections.impl
{
    public class GameListSection : BaseSection
    {
        private string username;
        private List<Game> games;

        public GameListSection(string username)
        {
            this.username = username;
            this.games = GameDatabseManager.GetAllGames();
        }

        public override string Title => "Игры";

        public override FrameworkElement Render()
        {
            var scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Background = Brushes.Transparent
            };

            var mainStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Background = Brushes.Transparent,
                Margin = new Thickness(20)
            };

            mainStackPanel.Children.Add(new TextBlock
            {
                Text = $"Добро пожаловать, {username}!",
                Foreground = Brushes.White,
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 20)
            });

            var wrapPanel = new WrapPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(0, 10, 0, 0)
            };

            foreach (var game in games)
            {
                var gameCard = CreateGameCard(game);
                wrapPanel.Children.Add(gameCard);
            }

            mainStackPanel.Children.Add(wrapPanel);
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
                Width = 280,
                Height = 400,
                Margin = new Thickness(10),
                Padding = new Thickness(15)
            };

            var cardStackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            var imageBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(60, 60, 60)),
                Height = 160,
                CornerRadius = new CornerRadius(6),
                Margin = new Thickness(0, 0, 0, 15)
            };

            try
            {
                if (!string.IsNullOrEmpty(game.Logo))
                {
                    var image = new Image
                    {
                        Source = new BitmapImage(new Uri(game.Logo, UriKind.RelativeOrAbsolute)),
                        Stretch = Stretch.Uniform,
                        Height = 160
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
            }
            catch
            {
                imageBorder.Child = new TextBlock
                {
                    Text = "Image Error",
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
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 8)
            };
            cardStackPanel.Children.Add(titleTextBlock);

            var genreTextBlock = new TextBlock
            {
                Text = game.Genre,
                Foreground = new SolidColorBrush(Color.FromRgb(180, 180, 180)),
                FontSize = 12,
                FontStyle = FontStyles.Italic,
                Margin = new Thickness(0, 0, 0, 8)
            };
            cardStackPanel.Children.Add(genreTextBlock);

            var ratingStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 8)
            };

            var ratingText = new TextBlock
            {
                Text = game.GetRatingStars(),
                Foreground = Brushes.Gold,
                FontSize = 14,
                VerticalAlignment = VerticalAlignment.Center
            };

            ratingStackPanel.Children.Add(ratingText);
            cardStackPanel.Children.Add(ratingStackPanel);

            var descriptionTextBlock = new TextBlock
            {
                Text = TruncateDescription(game.Description, 120),
                Foreground = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap,
                Height = 60,
                Margin = new Thickness(0, 0, 0, 15)
            };
            cardStackPanel.Children.Add(descriptionTextBlock);

            var priceButtonStackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var priceTextBlock = new TextBlock
            {
                Text = game.GetFormattedPrice(),
                Foreground = new SolidColorBrush(Color.FromRgb(16, 139, 239)), // #108BEF
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 10, 0)
            };

            var buyButton = new Button
            {
                Content = "В корзину",
                Background = new SolidColorBrush(Color.FromRgb(16, 139, 239)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                FontSize = 12,
                FontWeight = FontWeights.Bold,
                Cursor = System.Windows.Input.Cursors.Hand
            };

            buyButton.MouseEnter += (s, e) =>
            {
                buyButton.Background = new SolidColorBrush(Color.FromRgb(20, 120, 220));
            };
            buyButton.MouseLeave += (s, e) =>
            {
                buyButton.Background = new SolidColorBrush(Color.FromRgb(16, 139, 239));
            };

            priceButtonStackPanel.Children.Add(priceTextBlock);
            priceButtonStackPanel.Children.Add(buyButton);

            cardStackPanel.Children.Add(priceButtonStackPanel);

            cardBorder.Child = cardStackPanel;
            return cardBorder;
        }

        private string TruncateDescription(string description, int maxLength)
        {
            if (string.IsNullOrEmpty(description) || description.Length <= maxLength)
                return description;

            return description.Substring(0, maxLength) + "...";
        }
    }
}