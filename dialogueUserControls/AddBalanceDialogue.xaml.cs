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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GamesShop.dialogueUserControls
{

    public partial class AddBalanceDialogue : Window
    {
        public event Action<decimal> BalanceAdded;
        public event Action DialogCancelled;

        public AddBalanceDialogue()
        {
            InitializeComponent();
        }

        private void QuickAmount_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string amountStr)
            {
                AmountTextBox.Text = $"{amountStr}";
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(AmountTextBox.Text, out decimal amount) && amount > 0)
            {
                BalanceAdded?.Invoke(amount);
                ShowMessage("Успех", $"Баланс успешно пополнен на сумму {amount}₽ !");

            }
            else
            {
                ShowMessage("Ошибка", "Пожалуйста, введите корректную сумму");
            }
        }

        private void ShowMessage(string title, string message)
        {
            var dialog = new MessageDialogue(message);
            dialog.Title = title;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.Owner = Application.Current.MainWindow;

            if (FindResource("DialogWindowStyle") is Style dialogStyle)
            {
                dialog.Style = dialogStyle;
            }

            dialog.ShowDialog();
        }

        private Window CreateDialogWindow(string title, Window content)
        {
            content.Title = title;
            content.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            content.Owner = Application.Current.MainWindow;
            content.Style = (Style)FindResource("DialogWindowStyle");
            content.ShowDialog();

            return content;
        }
    }
}
