using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

namespace GamesShop.dialogueUserControls
{
    public partial class InputDialog : Window
    {
        public string InputText => InputTextBox.Text;
        public Func<string, bool> Validator { get; set; } 

        public InputDialog(string title = "Ввод данных", string prompt = "Введите текст:", string defaultValue = "", Func<string, bool> validationFunc = null)
        {
            InitializeComponent();

            TitleText.Text = title;
            PromptText.Text = prompt;
            InputTextBox.Text = defaultValue;
            Validator = validationFunc;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}