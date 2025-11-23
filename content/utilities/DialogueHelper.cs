using GamesShop.content.models;
using GamesShop.dialogueUserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GamesShop.content.utilities
{
    internal class DialogueHelper
    {
        public static void ShowMessage(string title, string message)
        {
            var dialog = new MessageDialogue(message);
            ShowDialogWindow(title, dialog);
        }

        public static void ShowConfirmation(string title, string message, Action<bool> onResult)
        {
            var dialog = new ConfirmationDialogue(message);
            dialog.DialogResult += onResult;
            ShowDialogWindow(title, dialog, () => dialog.DialogResult -= onResult);
        }

        public static void ShowAddReviewDialog(string title, Action<Review> onReviewAdded)
        {
            var dialog = new AddReviewDialogue();
            dialog.ReviewSubmitted += onReviewAdded;
            dialog.DialogCancelled += () => CloseDialog(dialog);

            ShowDialogWindow(title, dialog, () =>
            {
                dialog.ReviewSubmitted -= onReviewAdded;
                dialog.DialogCancelled -= () => CloseDialog(dialog);
            });
        }

        public static void ShowAddBalanceDialog(string title, Action<decimal> onBalanceAdded)
        {
            var dialog = new AddBalanceDialogue();
            dialog.BalanceAdded += onBalanceAdded;
            dialog.DialogCancelled += () => CloseDialog(dialog);

            ShowDialogWindow(title, dialog, () =>
            {
                dialog.BalanceAdded -= onBalanceAdded;
                dialog.DialogCancelled -= () => CloseDialog(dialog);
            });
        }

        private static void ShowDialogWindow(string title, Window content, Action cleanupAction = null)
        {
            content.Title = title;
            content.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            content.Owner = Application.Current.MainWindow;

            content.Closed += (s, e) =>
            {
                cleanupAction?.Invoke();
            };

            content.ShowDialog();
        }

        private static void CloseDialog(Window dialog)
        {
            dialog?.Close();
        }
    }
}