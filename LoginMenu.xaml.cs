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

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите имя пользователя и пароль.");
                return;
            }

            bool isValid = DatabaseManager.ValidateUserByUsername(username, password);

            if (isValid)
            {
                MessageBox.Show("Вход выполнен успешно!");
                Main mainWindow = new Main(username);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неправильное имя пользователя или пароль.");
            }
        }


        private void GoToRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterMenu registerMenu = new RegisterMenu();
            registerMenu.Show();

            this.Close();
        }
    }
}
