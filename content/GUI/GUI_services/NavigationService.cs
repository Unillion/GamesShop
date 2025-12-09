using GamesShop.content.models;
using System.Windows;
using System.Windows.Controls;

namespace GamesShop.content.GUI.GUI_services
{
    public class NavigationService
    {
        private readonly MainWindowControl mainWindow;
        private Stack<UIElement> navigationStack;

        public Action OnCartUpdated { get; set; }

        public NavigationService(MainWindowControl mainWindow)
        {
            this.mainWindow = mainWindow;
            navigationStack = new Stack<UIElement>();
        }

        public void ShowGamesSection()
        {
            HideAllSections();
            mainWindow.GamesSection.Visibility = Visibility.Visible;
            UpdateButtonStates(mainWindow.GamesButton);
            mainWindow.RefreshGames();
        }

        public void ShowLibrarySection()
        {
            HideAllSections();
            mainWindow.LibrarySection.Visibility = Visibility.Visible;
            UpdateButtonStates(mainWindow.LibraryButton);
        }

        public void ShowCartSection()
        {
            HideAllSections();
            mainWindow.CartSection.Visibility = Visibility.Visible;
            mainWindow.BottomBorder.Visibility = Visibility.Visible;
            UpdateButtonStates(mainWindow.CartButton);
            mainWindow.RefreshCart();
        }

        public void ShowProfileSection()
        {
            HideAllSections();
            mainWindow.ProfileSection.Visibility = Visibility.Visible;
            UpdateButtonStates(null);
        }

        public void ShowGameDetails(Game game)
        {
            navigationStack.Push(GetCurrentVisibleSection());
            HideAllSections();
            mainWindow.GameDetailsSection.Visibility = Visibility.Visible;
            SetNavigationEnabled(false);
            UpdateButtonStates(null);
        }

        public void ShowGameLibDetails(Game game)
        {
            navigationStack.Push(GetCurrentVisibleSection());
            HideAllSections();
            mainWindow.GameLibDetailsSection.Visibility = Visibility.Visible;
            SetNavigationEnabled(false);
            UpdateButtonStates(null);
        }
        public void ReturnToPreviousView(bool isGameDetails)
        {
            SetNavigationEnabled(true);
            if (isGameDetails)
                mainWindow.GameDetailsSection.Visibility = Visibility.Collapsed;
            else mainWindow.GameLibDetailsSection.Visibility = Visibility.Collapsed;

            OnCartUpdated?.Invoke();

            if (navigationStack.Count > 0)
            {
                var previousSection = navigationStack.Pop();
                previousSection.Visibility = Visibility.Visible;

                if (previousSection == mainWindow.GamesSection)
                {
                    UpdateButtonStates(mainWindow.GamesButton);
                    mainWindow.RefreshGames();
                }
                else if (previousSection == mainWindow.LibrarySection)
                    UpdateButtonStates(mainWindow.LibraryButton);
                else if (previousSection == mainWindow.CartSection)
                {
                    UpdateButtonStates(mainWindow.CartButton);
                    mainWindow.RefreshGames();
                }
                else if (previousSection == mainWindow.ProfileSection)
                    UpdateButtonStates(null);
            }
            else
            {
                ShowGamesSection();
            }
        }

        private void HideAllSections()
        {
            mainWindow.GamesSection.Visibility = Visibility.Collapsed;
            mainWindow.LibrarySection.Visibility = Visibility.Collapsed;
            mainWindow.CartSection.Visibility = Visibility.Collapsed;
            mainWindow.ProfileSection.Visibility = Visibility.Collapsed;
            mainWindow.GameDetailsSection.Visibility = Visibility.Collapsed;
            mainWindow.GameLibDetailsSection.Visibility = Visibility.Collapsed;
            mainWindow.BottomBorder.Visibility = Visibility.Collapsed;
        }

        private void UpdateButtonStates(Button activeButton)
        {
            mainWindow.GamesButton.Tag = "";
            mainWindow.LibraryButton.Tag = "";
            mainWindow.CartButton.Tag = "";

            if (activeButton != null)
                activeButton.Tag = "Active";
        }

        private void SetNavigationEnabled(bool enabled)
        {
            var navButtons = new[] {
                mainWindow.GamesButton,
                mainWindow.LibraryButton,
                mainWindow.CartButton,
                mainWindow.ProfileButtonControl
            };

            foreach (var button in navButtons)
            {
                if (button != null)
                {
                    button.IsEnabled = enabled;
                    button.Opacity = enabled ? 1.0 : 0.5;
                }
            }

            if (mainWindow.BackButton != null)
            {
                mainWindow.BackButton.Visibility = enabled ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private UIElement GetCurrentVisibleSection()
        {
            UIElement[] sections = new UIElement[] {
                mainWindow.GamesSection,
                mainWindow.LibrarySection,
                mainWindow.CartSection,
                mainWindow.ProfileSection,
                mainWindow.GameLibDetailsSection
            };

            return sections.FirstOrDefault(s => s.Visibility == Visibility.Visible) ?? mainWindow.GamesSection;
        }
    }
}
