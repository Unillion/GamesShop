using GamesShop.content.db;
using GamesShop.content.GUI.GUI_services;
using GamesShop.content.models;
using GamesShop.content.utilities;
using GamesShop.dialogueUserControls;
using Microsoft.IdentityModel.Tokens;
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
using System.Windows.Shapes;
using static Azure.Core.HttpHeader;

namespace GamesShop
{
    public partial class MainWindowControl : UserControl
    {
        private string username;
        private int userID;
        private List<Game> games;
        private List<Game> cartGames;
        private Game currentGame;

        private NavigationService navigationService;
        private GameService gameService;
        private CartService cartService;
        private GameDetailsService gameDetailsService;
        private LibraryService libraryService;
        private GameLibraryDetailsService gameLibraryDetailsService;
        private ProfileService profileService;

        public MainWindowControl(string username)
        {
            InitializeComponent();
            this.username = username;
            this.userID = UserDatabaseManager.GetUserId(username);

            navigationService = new NavigationService(this);

            UpdateServicesUsername(username);
            InitializeData();
            RefreshUIWithNewUsername(username);
            ShowGamesSection();

            BalanceManager.UserBalanceChanged += OnUserBalanceChanged;
        }

        private void InitializeData()
        {
            games = GameDatabseManager.GetAllGames();
            UserBalanceText.Text = UserDatabaseManager.GetUserBalance(username).ToString() + "₽";
            cartGames = UserDatabaseManager.GetUserCart(username);

            ProfileUsernameText.Text = username;
        }

        private void RefreshUIWithNewUsername(string newUsername)
        {
            WelcomeText.Text = $"Добро пожаловать, {newUsername}!";
            CartTitleText.Text = $"Корзина пользователя {newUsername}";

            InitializeData();

            if (ProfileSection.Visibility == Visibility.Visible)
            {
                ShowProfileSection();
            }
        }

        private void UpdateServicesUsername(string newUsername)
        {
            gameService = new GameService(newUsername, userID);
            cartService = new CartService(newUsername, this);
            gameDetailsService = new GameDetailsService(newUsername, userID);
            libraryService = new LibraryService(newUsername);
            gameLibraryDetailsService = new GameLibraryDetailsService(newUsername, userID);
            profileService = new ProfileService(newUsername, userID);

            InitializeEventHandlers();
            InitializeGameDetailsService();
        }

        private void InitializeEventHandlers()
        {
            gameService.OnGameCardClick = ShowGameDetails;
            cartService.OnGameCardClick = ShowGameDetails;

            gameService.OnCartUpdated = OnCartUpdated;
            navigationService.OnCartUpdated = OnCartUpdated;
            cartService.OnCartUpdated = OnCartUpdated;

            libraryService.OnCartUpdated = OnCartUpdated;
            libraryService.OnGameCardClick = ShowGameLibDetails;
        }

        private void InitializeGameDetailsService()
        {
            gameDetailsService.GameDetailsTitle = GameDetailsTitle;
            gameDetailsService.GameDetailsGenre = GameDetailsGenre;
            gameDetailsService.GameDetailsPrice = GameDetailsPrice;
            gameDetailsService.GameDetailsDescription = GameDetailsDescription;
            gameDetailsService.GameDetailsRating = GameDetailsRating;
            gameDetailsService.GameDetailsImage = GameDetailsImage;
            gameDetailsService.GameDetailsReleaseDate = GameDetailsReleaseDate;
            gameDetailsService.GameDetailsFeatures = GameDetailsFeatures;
            gameDetailsService.GameDetailsReviewsList = GameDetailsReviewsList;
            gameDetailsService.GameDetailsReviewsCount = GameDetailsReviewsCount;
            gameDetailsService.AddToCartDetailsButton = AddToCartDetailsButton;

            gameLibraryDetailsService.GameDetailsTitle = GameLibDetailsTitle;
            gameLibraryDetailsService.GameDetailsGenre = GameLibDetailsGenre;
            gameLibraryDetailsService.GameDetailsDescription = GameLibDetailsDescription;
            gameLibraryDetailsService.GameDetailsImage = GameLibDetailsImage;
            gameLibraryDetailsService.GameDetailsFeatures = GameLibDetailsFeatures;
            gameLibraryDetailsService.GameDetailsReleaseDate = GameLibDetailsReleaseDate;
            gameLibraryDetailsService.GameLibraryAcivationKey = ActivationKey;
            gameLibraryDetailsService.GameDetailsStatistics = GameStatisticsPanelText;

            profileService.ProfileUsernameText = ProfileUsernameText;
            profileService.ProfileBalanceText = ProfileBalanceText;
            profileService.ProfileRegistrationDate = ProfileRegistrationDate;
            profileService.GamesInLibraryCount = GamesInLibraryCount;
            profileService.ProfileEmailText = ProfileEmailText;
            profileService.ReviewsWrittenCount = ReviewsWrittenCount;
            profileService.TotalSpentAmount = TotalSpentAmount;
            profileService.TotalIncomeAmount = TotalIncomeAmount;
        }

        private void OnUserBalanceChanged(string username, decimal newBalance)
        {
            if (username == this.username)
            {
                RefreshBalance(newBalance.ToString("F2"));
            }
        }

        private void GamesButton_Click(object sender, RoutedEventArgs e)
        {
            ShowGamesSection();
        }

        private void LibraryButton_Click(object sender, RoutedEventArgs e)
        {
            ShowLibrarySection();
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            ShowCartSection();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ShowProfileSection();
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            cartService.Checkout();
            ShowCartSection();
        }

        private void ShowGamesSection()
        {
            navigationService.ShowGamesSection();
            gameService.RenderGames(GamesItemsControl);
        }

        private void ShowLibrarySection()
        {
            navigationService.ShowLibrarySection();
            libraryService.RenderLibrary(LibraryItemsControl, LibraryEmptytext);
        }

        private void ShowCartSection()
        {
            navigationService.ShowCartSection();
            cartService.RenderCart(CartItemsControl, CartEmptyText, TotalPriceText, CheckoutButton);
        }

        private void ShowProfileSection()
        {
            navigationService.ShowProfileSection();
            profileService.LoadProfileInformation();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            navigationService.ReturnToPreviousView(true);
        }

        private void AddToCartDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            gameDetailsService.AddToCart();
        }

        private void AddReviewButton_Click(object sender, RoutedEventArgs e)
        {
            gameDetailsService.AddReview();
        }

        public void ShowGameDetails(Game game)
        {
            currentGame = game;
            navigationService.ShowGameDetails(game);
            gameDetailsService.LoadGameDetails(game);
        }

        public void ShowGameLibDetails(Game game)
        {
            currentGame = game;
            navigationService.ShowGameLibDetails(game);
            gameLibraryDetailsService.LoadGameDetails(game);
            LoadGameStatistics(game.ID);
            createRandomKey();
        }

        public void RefreshGames()
        {
            gameService.RenderGames(GamesItemsControl);
        }

        public void RefreshCart()
        {
            cartService.RenderCart(CartItemsControl, CartEmptyText, TotalPriceText, CheckoutButton);
        }

        public void RefreshBalance(string newBalance)
        {
            UserBalanceText.Text = newBalance + "₽";
        }

        private void OnCartUpdated()
        {
            if (currentGame != null && GameDetailsSection.Visibility == Visibility.Visible)
            {
                gameDetailsService.UpdateAddToCartButton();
            }
            if (CartSection.Visibility == Visibility.Visible)
            {
                RefreshCart();
            }
            if (GamesSection.Visibility == Visibility.Visible)
            {
                RefreshGames();
            }
        }

        private void AddToBalance_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddBalanceDialogue();
            dialog.BalanceAdded += (amount) =>
            {
                BalanceManager.UpdateBalance(username, amount);
                UserDatabaseManager.UpdateMultipleStats(userID, income: amount);
                profileService.LoadProfileInformation();
            };

            dialog.ShowDialog();
        }

        private void BackFromLibButton_Click(object sender, RoutedEventArgs e)
        {
            navigationService.ReturnToPreviousView(false);
        }

        private void LoadGameStatistics(int gameID)
        {
            var statistics = GameDatabseManager.GetGameStatistics(gameID);

            string statsText;

            if (statistics == null)
            {
                statsText = "Статистика недоступна";
            }
            else
            {
                statsText = $"🛒 Всего покупок: {statistics.TotalPurchases}\n" +
                           $"🔑 Обновлений ключей: {statistics.UsersKeyRefreshes}\n" +
                           $"📅 Последнее обновление: {statistics.LastUpdated:dd.MM.yyyy HH:mm}";
            }

            GameStatisticsPanelText.Text = statsText;
            GameStatisticsPanelText.Foreground = Brushes.White;
            GameStatisticsPanelText.FontStyle = FontStyles.Normal;

            UpdateActivationKey.Tag = gameID;
        }

        private void createRandomKey()
        {
            ActivationKey.Text = Guid.NewGuid().ToString("D").ToUpper().Substring(0, 23);
        }

        private void UpdateActivationKey_Click(object sender, RoutedEventArgs e)
        {
            createRandomKey();

            int gameID = Convert.ToInt32(UpdateActivationKey.Tag);
            GameDatabseManager.IncrementKeyRefreshes(gameID);
            LoadGameStatistics(gameID);
        }

        private void CopyActivationKey_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ActivationKey.Text);
        }

        private void ChangeName_Click(object sender, RoutedEventArgs e)
        {
            var newUsername = UserManager.changeUsername(userID, username);
            if (!string.IsNullOrEmpty(newUsername))
            {
                username = newUsername;

                UpdateServicesUsername(newUsername);

                profileService.LoadProfileInformation();
                RefreshUIWithNewUsername(newUsername);
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            bool result = false;
            DialogueHelper.ShowConfirmation("Выйти?", "Вы уверены что хотите выйти?", a => result = a);

            if (result)
            {
                Window currentWindow = Window.GetWindow(this);

                LoginMenu loginWindow = new LoginMenu();
                loginWindow.Show();

                currentWindow?.Close();
            }
        }
    }
}