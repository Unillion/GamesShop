using GamesShop.content.models;
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
    public partial class AddReviewDialogue : Window
    {
        public event Action<Review> ReviewSubmitted;
        public event Action DialogCancelled;

        public AddReviewDialogue()
        {
            InitializeComponent();

            for (decimal i = 1; i <= 5; i += 0.5m)
            {
                RatingComboBox.Items.Add($"{i:0.0} ★");
            }
            RatingComboBox.SelectedIndex = 8;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogCancelled?.Invoke();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ReviewTextBox.Text))
            {
                ShowMessage("Ошибка", "Пожалуйста, напишите текст отзыва");
                return;
            }

            if (ReviewTextBox.Text.Length < 10)
            {
                ShowMessage("Ошибка", "Отзыв должен содержать минимум 10 символов");
                return;
            }

            var review = new Review
            {
                ReviewText = ReviewTextBox.Text.Trim(),
                Rating = (int)decimal.Parse(RatingComboBox.SelectedItem.ToString().Split(' ')[0]),
                ReviewDate = DateTime.Now
            };

            ReviewSubmitted?.Invoke(review);
            this.Close();
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
