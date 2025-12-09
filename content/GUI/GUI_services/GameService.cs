using GamesShop.content.db;
using GamesShop.content.GUI.factories;
using GamesShop.content.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GamesShop.content.GUI.GUI_services
{
    public class GameService
    {
        private readonly string username;
        private readonly int userId;
        private List<Game> allGames;
        private List<Game> filteredGames;

        public Game CurrentGame { get; private set; }
        public Action<Game> OnGameCardClick { get; set; }
        public Action OnCartUpdated { get; set; }
        public Action<int> OnGamesFiltered { get; set; }

        public string CurrentSearchText { get; private set; } = "";
        public string CurrentSortBy { get; private set; } = "NameAsc";

        public GameService(string username, int userId)
        {
            this.username = username;
            this.userId = userId;
            LoadGames();
            filteredGames = allGames;
        }

        public void LoadGames()
        {
            allGames = GameDatabseManager.GetAllGames();
            filteredGames = allGames;
            ApplySorting();
        }

        public void RenderGames(ItemsControl gamesItemsControl)
        {
            gamesItemsControl.Items.Clear();

            foreach (var game in filteredGames)
            {
                var gameCard = GameCardFactory.CreateGameCard(
                    game,
                    false, false,
                    username,
                    OnGameCardClick,
                    OnAddToCart
                );
                gamesItemsControl.Items.Add(gameCard);
            }

            OnGamesFiltered?.Invoke(filteredGames.Count);
        }

        public void ShowGameDetails(Game game, FrameworkElement detailsSection)
        {
            CurrentGame = game;
        }

        public void SearchGames(string searchText)
        {
            CurrentSearchText = searchText?.Trim()?.ToLower() ?? "";
            ApplyFiltering();
        }

        public void SortGames(string sortBy)
        {
            CurrentSortBy = sortBy;
            ApplySorting();
        }

        public void ResetFilters()
        {
            CurrentSearchText = "";
            CurrentSortBy = "NameAsc";
            filteredGames = allGames.ToList();
            ApplySorting();
        }

        private void ApplyFiltering()
        {
            if (string.IsNullOrWhiteSpace(CurrentSearchText))
            {
                filteredGames = allGames.ToList();
            }
            else
            {
                filteredGames = allGames
                    .Where(game =>
                        game.Title.ToLower().Contains(CurrentSearchText) ||
                        game.GameGenres.ToString().ToLower().Contains(CurrentSearchText) ||
                        game.Description.ToLower().Contains(CurrentSearchText) ||
                        game.GameDevelopers.ToString().ToLower().Contains(CurrentSearchText))
                    .ToList();
            }

            ApplySorting();
        }

        private void ApplySorting()
        {
            switch (CurrentSortBy)
            {
                case "NameAsc":
                    filteredGames = filteredGames.OrderBy(g => g.Title).ToList();
                    break;
                case "NameDesc":
                    filteredGames = filteredGames.OrderByDescending(g => g.Title).ToList();
                    break;
                case "DateDesc":
                    filteredGames = filteredGames.OrderByDescending(g => g.ReleaseDate).ToList();
                    break;
                case "DateAsc":
                    filteredGames = filteredGames.OrderBy(g => g.ReleaseDate).ToList();
                    break;
                case "PriceDesc":
                    filteredGames = filteredGames.OrderByDescending(g => g.Price).ToList();
                    break;
                case "PriceAsc":
                    filteredGames = filteredGames.OrderBy(g => g.Price).ToList();
                    break;
                case "RatingDesc":
                    filteredGames = filteredGames.OrderByDescending(g => g.Rating).ToList();
                    break;
                case "RatingAsc":
                    filteredGames = filteredGames.OrderBy(g => g.Rating).ToList();
                    break;
                default:
                    filteredGames = filteredGames.OrderBy(g => g.Title).ToList();
                    break;
            }
        }

        public List<Game> GetFilteredGames()
        {
            return filteredGames;
        }

        public int GetTotalGamesCount()
        {
            return allGames?.Count ?? 0;
        }

        public int GetFilteredGamesCount()
        {
            return filteredGames?.Count ?? 0;
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
            return allGames;
        }

        public Game GetGameById(int gameId)
        {
            return allGames.FirstOrDefault(g => g.ID == gameId);
        }
    }
}