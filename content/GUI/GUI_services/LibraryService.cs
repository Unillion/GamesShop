using GamesShop.content.db;
using GamesShop.content.GUI.factories;
using GamesShop.content.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GamesShop.content.GUI.GUI_services
{
    public class LibraryService
    {
        private readonly string username;
        private List<Game> libraryGames;
        public Action<Game> OnGameCardClick { get; set; }
        public Action OnCartUpdated { get; set; }

        public LibraryService(string username)
        {
            this.username = username;
            LoadLibrary();
        }
        
        private void LoadLibrary()
        {
            libraryGames = UserDatabaseManager.GetUserLibrary(username);
        }

        public void RenderLibrary(ItemsControl cartItemsControl, TextBlock emptyText)
        {
            LoadLibrary();

            if (libraryGames.Count == 0)
            {
                ShowEmptyCart(emptyText, cartItemsControl);
            }
            else
            {
                ShowCartWithItems(cartItemsControl, emptyText);
            }
        }

        private void ShowEmptyCart(TextBlock emptyText, ItemsControl cartItemsControl)
        {
            emptyText.Visibility = Visibility.Visible;
            cartItemsControl.Items.Clear();
        }

        private void ShowCartWithItems(ItemsControl cartItemsControl, TextBlock emptyText)
        {
            emptyText.Visibility = Visibility.Collapsed;

            decimal totalPrice = libraryGames.Sum(g => g.Price);

            RenderLibraryItems(cartItemsControl);
        }

        private void RenderLibraryItems(ItemsControl libItemsControl)
        {
            libItemsControl.Items.Clear();
            foreach (var game in libraryGames)
            {
                var gameCard = GameCardFactory.CreateGameCard(
                    game,
                    false,true,
                    username,
                    OnGameCardClickInternal,
                    null
                );
                libItemsControl.Items.Add(gameCard);
            }
        }

        private void OnGameCardClickInternal(Game game)
        {
            OnGameCardClick?.Invoke(game);
        }
    }
}
