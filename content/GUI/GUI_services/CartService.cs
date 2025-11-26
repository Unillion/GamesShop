using GamesShop.content.db;
using GamesShop.content.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using GamesShop.content.GUI.factories;
using GamesShop.content.utilities;

namespace GamesShop.content.GUI.GUI_services
{
    public class CartService
    {
        private readonly string username;
        private List<Game> cartGames;
        public Action<Game> OnGameCardClick { get; set; }
        public Action OnCartUpdated { get; set; }
        private MainWindowControl window;

        public CartService(string username, MainWindowControl mainWindow)
        {
            this.username = username;
            this.window = mainWindow;
            LoadCart();
        }

        public void LoadCart()
        {
            cartGames = UserDatabaseManager.GetUserCart(username);
        }

        public void RenderCart(ItemsControl cartItemsControl, TextBlock emptyText,
                              TextBlock totalPriceText, Button checkoutButton)
        {
            LoadCart();

            if (cartGames.Count == 0)
            {
                ShowEmptyCart(emptyText, totalPriceText, checkoutButton, cartItemsControl);
            }
            else
            {
                ShowCartWithItems(cartItemsControl, emptyText, totalPriceText, checkoutButton);
            }
        }

        private void ShowEmptyCart(TextBlock emptyText, TextBlock totalPriceText,
                                 Button checkoutButton, ItemsControl cartItemsControl)
        {
            emptyText.Visibility = Visibility.Visible;
            totalPriceText.Visibility = Visibility.Collapsed;
            checkoutButton.Visibility = Visibility.Collapsed;
            cartItemsControl.Items.Clear();
        }

        private void ShowCartWithItems(ItemsControl cartItemsControl, TextBlock emptyText,
                                     TextBlock totalPriceText, Button checkoutButton)
        {
            emptyText.Visibility = Visibility.Collapsed;
            totalPriceText.Visibility = Visibility.Visible;
            checkoutButton.Visibility = Visibility.Visible;

            decimal totalPrice = cartGames.Sum(g => g.Price);
            totalPriceText.Text = $"Общая стоимость: {totalPrice:F2} $";

            RenderCartItems(cartItemsControl);
        }

        private void RenderCartItems(ItemsControl cartItemsControl)
        {
            cartItemsControl.Items.Clear();
            foreach (var game in cartGames)
            {
                var gameCard = GameCardFactory.CreateGameCard(
                    game,
                    true, false,
                    username,
                    OnGameCardClickInternal,
                    OnRemoveFromCart
                );
                cartItemsControl.Items.Add(gameCard);
            }
        }

        private void OnGameCardClickInternal(Game game)
        {
            OnGameCardClick?.Invoke(game);
        }

        public void Checkout()
        {
            if (cartGames.Count == 0)
            {
                MessageBox.Show("Корзина пуста!", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            decimal totalPrice = cartGames.Sum(g => g.Price);

            var result = false;
            DialogueHelper.ShowConfirmation(
            $"Подтверждение заказа",
            $"Вы уверены, что хотите оформить заказ на сумму {totalPrice:F2} $?", (actiob) => { if (actiob) result = true; });

            if (result && UserDatabaseManager.GetUserBalance(username) >= totalPrice)
            {
                foreach (var game in cartGames)
                {
                    UserDatabaseManager.RemoveGameFromCart(username, game.ID);
                    UserDatabaseManager.AddGameToLibrary(username, game.ID);
                    UserDatabaseManager.removeFromUserBalance(username, totalPrice);

                    GameDatabseManager.IncrementPurchaseCount(game.ID);

                    window.RefreshBalance(UserDatabaseManager.GetUserBalance(username).ToString());
                }

                LoadCart();
                OnCartUpdated?.Invoke();
                DialogueHelper.ShowMessage("Оформление", "Все игры успешно куплены!");
            }
            else DialogueHelper.ShowMessage("Ошибка", "Недостаточно средств! :(");
        }

        public void RemoveFromCart(int gameId)
        {
            try
            {
                UserDatabaseManager.RemoveGameFromCart(username, gameId);
                LoadCart();
                OnCartUpdated?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении из корзины: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool IsGameInCart(int gameId)
        {
            return cartGames.Any(g => g.ID == gameId);
        }

        public int GetCartItemsCount()
        {
            return cartGames.Count;
        }

        public decimal GetTotalPrice()
        {
            return cartGames.Sum(g => g.Price);
        }

        private void OnRemoveFromCart(int gameId)
        {
            RemoveFromCart(gameId);
        }
    }
}
