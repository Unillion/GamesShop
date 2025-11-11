using GamesShop.content.models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace GamesShop.content.db
{
    public class GameDatabseManager
    {
        public static List<Game> GetAllGames()
        {
            using (var context = new GameShopContext())
            {
                return context.Games
                    .AsNoTracking()
                    .ToList();
            }
        }

        public static Game GetGameById(int id)
        {
            using (var context = new GameShopContext())
            {
                return context.Games
                    .AsNoTracking()
                    .FirstOrDefault(g => g.ID == id);
            }
        }

        public static List<Game> GetGamesByGenre(string genre)
        {
            using (var context = new GameShopContext())
            {
                return context.Games
                    .AsNoTracking()
                    .Where(g => g.Genre == genre)
                    .ToList();
            }
        }

        public static List<Game> SearchGames(string searchTerm)
        {
            using (var context = new GameShopContext())
            {
                return context.Games
                    .AsNoTracking()
                    .Where(g => g.Title.Contains(searchTerm) || g.Description.Contains(searchTerm))
                    .ToList();
            }
        }

        public static bool AddGame(Game game)
        {
            using (var context = new GameShopContext())
            {
                try
                {
                    context.Games.Add(game);
                    return context.SaveChanges() > 0;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Ошибка при добавлении игры: {ex.Message}");
                    return false;
                }
            }
        }

        public static bool UpdateGame(Game game)
        {
            using (var context = new GameShopContext())
            {
                try
                {
                    var existingGame = context.Games.Find(game.ID);
                    if (existingGame != null)
                    {
                        context.Entry(existingGame).CurrentValues.SetValues(game);
                        return context.SaveChanges() > 0;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Ошибка при обновлении игры: {ex.Message}");
                    return false;
                }
            }
        }

        public static bool DeleteGame(int id)
        {
            using (var context = new GameShopContext())
            {
                try
                {
                    var game = context.Games.Find(id);
                    if (game != null)
                    {
                        context.Games.Remove(game);
                        return context.SaveChanges() > 0;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Ошибка при удалении игры: {ex.Message}");
                    return false;
                }
            }
        }
    }
}