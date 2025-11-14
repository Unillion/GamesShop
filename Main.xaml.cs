using GamesShop.content.db;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GamesShop
{
    public partial class Main : Window
    {
        private string currentUsername;


        public Main(string username)
        {
            InitializeComponent();
            currentUsername = username;

            var mainControl = new MainWindowControl(username);
            this.Content = mainControl;

        }
    }
}