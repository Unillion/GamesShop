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

    public partial class MessageDialogue : Window
    {
        public event Action DialogClosed;
        public MessageDialogue(string message)
        {
            InitializeComponent();
            MessageText.Text = message;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogClosed?.Invoke();
            this.DialogResult = true;
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            DialogClosed?.Invoke();
            base.OnClosed(e);
        }
    }
}
