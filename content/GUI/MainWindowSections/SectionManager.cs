using GamesShop.content.GUI.MainWindowSections.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace GamesShop.content.GUI.MainWindowSections
{
    public class SectionManager
    {
        private readonly ContentControl content;
        private readonly Dictionary<string, IContentSection> sections;
        private Button activeButton;
        private readonly List<Button> navigationButtons;

        public SectionManager(ContentControl content)
        {
            this.content = content;
            sections = new Dictionary<string, IContentSection>();
            navigationButtons = new List<Button>();
        }

        public void RegisterSection(string sectionKey, IContentSection section)
        {
            sections[sectionKey] = section;
        }

        public void RegisterNavigationButton(Button button)
        {
            navigationButtons.Add(button);
        }

        public void NavigateTo(string sectionKey, Button navigationButton = null)
        {
            if (sections.TryGetValue(sectionKey, out var section))
            {
                if (navigationButton != null)
                {
                    SetActiveButton(navigationButton);
                }

                content.Content = section.Render();
            }
            else
            {
                Console.WriteLine($"Секция '{sectionKey}' не найдена!");
            }
        }

        private void SetActiveButton(Button activeButton)
        {
            ResetAllNavigationButtons();
            this.activeButton = activeButton;

            activeButton.Tag = "Active";
            activeButton.Background = new SolidColorBrush(Color.FromRgb(70, 70, 70));

            activeButton.BorderThickness = new System.Windows.Thickness(0, 0, 0, 2);
            activeButton.BorderBrush = Brushes.DodgerBlue;
        }

        private void ResetAllNavigationButtons()
        {
            foreach (var button in navigationButtons)
            {
                button.Tag = null;
                button.Background = Brushes.Transparent;
                button.BorderThickness = new System.Windows.Thickness(0);
                button.BorderBrush = null;
            }
        }
    }
}