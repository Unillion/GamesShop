using GamesShop.content.db;
using GamesShop.content.GUI.GUI_services;
using GamesShop.content.models;
using GamesShop.content.utilities;
using GamesShop.dialogueUserControls;
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

        public MainWindowControl(string username)
        {
            InitializeComponent();
            this.username = username;
            this.userID = UserDatabaseManager.GetUserId(username);

            navigationService = new NavigationService(this);
            gameService = new GameService(username, userID);
            cartService = new CartService(username);
            gameDetailsService = new GameDetailsService(username, userID);

            InitializeGameDetailsService();
            InitializeEventHandlers();
            InitializeData();
            ShowGamesSection();

            BalanceManager.UserBalanceChanged += OnUserBalanceChanged;
        }

        private void InitializeData()
        {
            games = GameDatabseManager.GetAllGames();
            UserBalanceText.Text = UserDatabaseManager.GetUserBalance(username).ToString() + "₽";
            cartGames = UserDatabaseManager.GetUserCart(username);

            WelcomeText.Text = $"Добро пожаловать, {username}!";
            CartTitleText.Text = $"Корзина пользователя {username}";
            ProfileUsernameText.Text = username;
        }

        private void InitializeEventHandlers()
        {
            gameService.OnGameCardClick = ShowGameDetails;
            cartService.OnGameCardClick = ShowGameDetails;

            gameService.OnCartUpdated = OnCartUpdated;
            navigationService.OnCartUpdated = OnCartUpdated;
            cartService.OnCartUpdated = OnCartUpdated;
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
        }

        private void ShowCartSection()
        {
            navigationService.ShowCartSection();
            cartService.RenderCart(CartItemsControl, CartEmptyText, TotalPriceText, CheckoutButton);
        }

        private void ShowProfileSection()
        {
            navigationService.ShowProfileSection();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            navigationService.ReturnToPreviousView();
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
            };

            dialog.ShowDialog();
        }
    }

}
