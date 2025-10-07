using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            if (!canRegister) return;
            UserManager.RegisterUser(UsernameTextBox.Text, EmailTextBox.Text, ConfirmPasswordBox.Password);

            UsernameTextBox.Text = string.Empty;
            EmailTextBox.Text = string.Empty;
            PasswordBox.Password = string.Empty;

            MessageBox.Show("Регистрация прошла успешно. Войдите в систему");
            LoginMenu loginMenu = new LoginMenu();
            loginMenu.Show();

            this.Close();
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

            if (password != confPassword)
            {
                PasswordError.Visibility = Visibility.Visible;
            }
            else PasswordError.Visibility = Visibility.Hidden;
        }
    }
}