using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GamesShop
{
    public partial class Main : Window
    {
        private string currentUsername;
        public Main(string username)
        {
            InitializeComponent();

            // По умолчанию показываем раздел "Игры"
            ShowGamesSection();

            currentUsername = username;
            LoadUserBalance();
        }

        private void LoadUserBalance()
        {
            decimal balance = DatabaseManager.GetUserBalance(currentUsername, "USD");
            UserBalanceText.Text = balance.ToString("F2"); // Формат с двумя знаками после запятой
        }
        private void SetActive(Button activeButton)
        {
            // Сброс активного состояния
            GamesButton.Tag = null;
            AboutButton.Tag = null;

            // Установка активной кнопки
            activeButton.Tag = "Active";
        }

        private void ShowGamesSection()
        {
            SetActive(GamesButton);
            MainContent.Content = new TextBlock
            {
                Text = "Раздел: Игры",
                Foreground = Brushes.White,
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private void ShowAboutSection()
        {
            SetActive(AboutButton);
            MainContent.Content = new TextBlock
            {
                Text = "Раздел: О нас",
                Foreground = Brushes.White,
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        private void GamesButton_Click(object sender, RoutedEventArgs e)
        {
            ShowGamesSection();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAboutSection();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = new ContextMenu
            {
                Background = new SolidColorBrush(Color.FromRgb(45, 45, 45)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                PlacementTarget = ProfileButtonControl,
                Placement = PlacementMode.Bottom,
                StaysOpen = false
            };

            MenuItem profileItem = new MenuItem
            {
                Header = "Мой профиль",
                Icon = new Path
                {
                    Fill = Brushes.White,
                    Width = 16,
                    Height = 16
                }
            };
            profileItem.Click += (s, args) =>
            {
                DatabaseManager.UpdateUserBalance(currentUsername, "RU", 100);
            };

            MenuItem logoutItem = new MenuItem
            {
                Header = "Выйти",
                Icon = new Path
                {
                    Fill = Brushes.White,
                    Width = 16,
                    Height = 16
                }
            };
            logoutItem.Click += (s, args) =>
            {
            };

            // Добавляем пункты
            menu.Items.Add(profileItem);
            menu.Items.Add(logoutItem);

            menu.IsOpen = true;
        }
    }
}
