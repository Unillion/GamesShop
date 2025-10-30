using GamesShop.content.db;
using GamesShop.content.game;
using GamesShop.content.GUI.MainWindowSections;
using GamesShop.content.GUI.MainWindowSections.impl;
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
        private SectionManager sectionManager;

        public Main(string username)
        {
            InitializeComponent();
            currentUsername = username;
            InitializeNavigation();
            LoadUserBalance();
        }

        private void InitializeNavigation()
        {
            sectionManager = new SectionManager(MainContent);

            sectionManager.RegisterSection("Games", new GameListSection(currentUsername));
            sectionManager.RegisterSection("Library", new LibrarySection());

            sectionManager.RegisterNavigationButton(GamesButton);
            sectionManager.RegisterNavigationButton(AboutButton);

            sectionManager.NavigateTo("Games", GamesButton);
        }

        private void LoadUserBalance()
        {
            decimal balance = UserDatabaseManager.GetUserBalance(currentUsername, "USD");
            UserBalanceText.Text = balance.ToString("F2");
        }

        private void GamesButton_Click(object sender, RoutedEventArgs e)
        {
            sectionManager.NavigateTo("Games", (Button)sender);
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            sectionManager.NavigateTo("Library", (Button)sender);
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ShowProfileContextMenu();
        }

        private void ShowProfileContextMenu()
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
                UserDatabaseManager.UpdateUserBalance(currentUsername, "RU", 100);
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
                // Логика выхода
            };

            menu.Items.Add(profileItem);
            menu.Items.Add(logoutItem);
            menu.IsOpen = true;
        }
    }
}