using GamesShop.content.db;
using GamesShop.content.utilities;
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
using System.Windows.Shapes;

namespace GamesShop
{

    public partial class LoginMenu : Window
    {
        public LoginMenu()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username))
            {
                DialogueHelper.ShowMessage("Ошибка", "Введите имя пользователя");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                DialogueHelper.ShowMessage("Ошибка", "Введите пароль");
                return;
            }

            bool isValid = UserDatabaseManager.ValidateUserByUsername(username, password);

            if (isValid)
            {
                DialogueHelper.ShowMessage("Успех", "Вход успешно выполнен!");

                Window mainWindow = new Window
                {
                    Title = "GamesShop",
                    Content = new MainWindowControl(username),
                    Width = SystemParameters.PrimaryScreenWidth,
                    Height = SystemParameters.PrimaryScreenHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    WindowState = WindowState.Maximized,
                    WindowStyle = WindowStyle.None,
                    ResizeMode = ResizeMode.NoResize,
                    Topmost = false
                };

                mainWindow.Show();
                this.Close();
            }
            else
            {
                DialogueHelper.ShowMessage("Ошибка", "Неправильное имя пользователя или пароль!");
            }
        }


        private void GoToRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterMenu registerMenu = new RegisterMenu();
            registerMenu.Show();

            this.Close();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
