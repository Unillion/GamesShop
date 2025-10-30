using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using GamesShop.content.db;
using GamesShop.content.user;

namespace GamesShop
{
    public partial class RegisterMenu : Window
    {
        bool canRegister = true;

        public RegisterMenu()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
            {
                canRegister = false;
                return;
            }

            canRegister = true;

            User newUser = new User(UsernameTextBox.Text.Trim(),
                                    EmailTextBox.Text.Trim(),
                                    PasswordBox.Password);

            bool success = UserDatabaseManager.AddUser(newUser);

            if (success)
            {
                UsernameTextBox.Text = string.Empty;
                EmailTextBox.Text = string.Empty;
                PasswordBox.Password = string.Empty;
                ConfirmPasswordBox.Password = string.Empty;

                MessageBox.Show("Регистрация прошла успешно. Войдите в систему");
                LoginMenu loginMenu = new LoginMenu();
                loginMenu.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка при регистрации. Попробуйте снова.");
            }
        }


        private void GoToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginMenu loginWindow = new LoginMenu();
            loginWindow.Show();
            this.Close();
        }

        private void ChangedConfirmPassword(object sender, RoutedEventArgs e)
        {
            string password = PasswordBox.Password;
            string confPassword = ConfirmPasswordBox.Password;

            PasswordError.Visibility = password != confPassword
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        private bool ValidateInputs()
        {
            string username = UsernameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
                return false;

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return false;
            }
            

            if (password != confirmPassword)
            {
                PasswordError.Visibility = Visibility.Visible;
                return false;
            }else PasswordError.Visibility = Visibility.Hidden;

            return true;
        }
    }
}
