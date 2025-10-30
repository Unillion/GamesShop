using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GamesShop.content.GUI.MainWindowSections
{
    public interface IContentSection
    {
        FrameworkElement Render();
        string Title { get; }
    }
}
