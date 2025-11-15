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
    public class GameService
    {
        private readonly string username;
        private readonly int userId;
        private List<Game> games;

        public Game CurrentGame { get; private set; }
        public Action<Game> OnGameCardClick { get; set; }
        public Action OnCartUpdated { get; set; }

        public GameService(string username, int userId)
        {
            this.username = username;
            this.userId = userId;
            LoadGames();
        }

        public void LoadGames()
        {
            games = GameDatabseManager.GetAllGames();
        }

        public void RenderGames(ItemsControl gamesItemsControl)
        {
            LoadGames();

            gamesItemsControl.Items.Clear();
            foreach (var game in games)
            {
                var gameCard = GameCardFactory.CreateGameCard(
                    game,
                    false,
                    username,
                    OnGameCardClick,
                    OnAddToCart
                );
                gamesItemsControl.Items.Add(gameCard);
            }
        }

        public void ShowGameDetails(Game game, FrameworkElement detailsSection)
        {
            CurrentGame = game;
        }


        private void OnAddToCart(int gameId)
        {
            try
            {
                UserDatabaseManager.AddGameToCart(username, gameId);
                OnCartUpdated?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении в корзину: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public List<Game> GetAllGames()
        {
            return games;
        }

        public Game GetGameById(int gameId)
        {
            return games.FirstOrDefault(g => g.ID == gameId);
        }
    }
}
