using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace GamesShop.content.GUI.MainWindowSections.impl
{
    public class LibrarySection : BaseSection
    {
        public override string Title => "Библиотека игр";

        public override FrameworkElement Render()
        {
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                MaxWidth = 400
            };

            stackPanel.Children.Add(new TextBlock
            {
                Text = "Здесь ничего нет",
                Foreground = Brushes.White,
                FontSize = 24,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 20),
                HorizontalAlignment = HorizontalAlignment.Center
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = "Но вы всё еще можете приобрести игру!",
                Foreground = Brushes.White,
                FontSize = 16,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center
            });

            return stackPanel;
        }
    }
}
