using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GamesShop.content.GUI.MainWindowSections;

namespace GamesShop.content.GUI.MainWindowSections.impl
{
    public abstract class BaseSection : IContentSection
    {
        public abstract string Title { get; }

        public virtual FrameworkElement Render()
        {
            return new TextBlock
            {
                Text = $"Раздел: {Title}",
                Foreground = Brushes.White,
                FontSize = 20,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }
    }
}
