using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using GamesShop.content.db;
using GamesShop.content.models;
using GamesShop.content.utilities;

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
                                    PasswordBox.Password, DateTime.Now);

            var isUserExist = UserDatabaseManager.isUserExist(newUser.Username);
            if (isUserExist)
            {
                DialogueHelper.ShowMessage("Ошибка", "Такое имя пользователя уже существует!");
                return;
            }

            if (UserDatabaseManager.isMailExist(newUser.Email))
            {
                DialogueHelper.ShowMessage("Ошибка", "Такая почта уже используется!");
                return;
            }

            if (newUser.Password.Length < 6)
            {
                DialogueHelper.ShowMessage("Ошибка", "Пароль должен быть больше 6 знаков!");
                return;
            }

            if (newUser.Username.Length > 15)
            {
                DialogueHelper.ShowMessage("Ошибка", "Имя пользователя не должно быть больше 15 символов!");
                return;
            }

            bool success = UserDatabaseManager.AddUser(newUser);

            if (success)
            {
                UsernameTextBox.Text = string.Empty;
                EmailTextBox.Text = string.Empty;
                PasswordBox.Password = string.Empty;
                ConfirmPasswordBox.Password = string.Empty;

                DialogueHelper.ShowMessage("Успех", "Регистрация прошла успешно. Войдите в систему");
                LoginMenu loginMenu = new LoginMenu();
                loginMenu.Show();
                this.Close();
            }
            else
            {
                DialogueHelper.ShowMessage("Ошибка", "Ошибка при регистрации, попробуйте снова");
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
            }
            else PasswordError.Visibility = Visibility.Hidden;

            return true;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
