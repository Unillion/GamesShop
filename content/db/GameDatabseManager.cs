using GamesShop.content.models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static Game GetGameWithDetails(int id)
        {
            using (var context = new GameShopContext())
            {
                return context.Games
                    .AsNoTracking()
                    .Include(g => g.GameGenres)
                        .ThenInclude(gg => gg.Genre)
                    .Include(g => g.GameDevelopers)
                        .ThenInclude(gd => gd.Developer)
                    .Include(g => g.GameLanguages)
                        .ThenInclude(gl => gl.Language)
                    .Include(g => g.Reviews)
                        .ThenInclude(r => r.User)
                    .FirstOrDefault(g => g.ID == id);
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

        public static List<Genre> GetGameGenres(int gameId)
        {
            using (var context = new GameShopContext())
            {
                return context.GameGenres
                    .AsNoTracking()
                    .Where(gg => gg.GameID == gameId)
                    .Select(gg => gg.Genre)
                    .ToList();
            }
        }

        public static List<Developers> GetGameDevelopers(int gameId)
        {
            using (var context = new GameShopContext())
            {
                return context.GameDevelopers
                    .AsNoTracking()
                    .Where(gd => gd.GameID == gameId)
                    .Select(gd => gd.Developer)
                    .ToList();
            }
        }

        public static List<Language> GetGameLanguages(int gameId)
        {
            using (var context = new GameShopContext())
            {
                return context.GameLanguages
                    .AsNoTracking()
                    .Where(gl => gl.GameID == gameId)
                    .Select(gl => gl.Language)
                    .ToList();
            }
        }

        public static List<Review> GetGameReviews(int gameId)
        {
            using (var context = new GameShopContext())
            {
                return context.Reviews
                    .AsNoTracking()
                    .Where(r => r.GameID == gameId)
                    .Include(r => r.User)
                    .OrderByDescending(r => r.ReviewDate)
                    .ToList();
            }
        }

        public static List<Genre> GetAllGenres()
        {
            using (var context = new GameShopContext())
            {
                return context.Genres
                    .AsNoTracking()
                    .OrderBy(g => g.Name)
                    .ToList();
            }
        }

        public static List<Developers> GetAllDevelopers()
        {
            using (var context = new GameShopContext())
            {
                return context.Developers
                    .AsNoTracking()
                    .OrderBy(d => d.Name)
                    .ToList();
            }
        }

        public static List<Language> GetAllLanguages()
        {
            using (var context = new GameShopContext())
            {
                return context.Languages
                    .AsNoTracking()
                    .OrderBy(l => l.LanguageName)
                    .ToList();
            }
        }
        public static bool AddReview(Review review)
        {
            using (var context = new GameShopContext())
            {
                try
                {
                    context.Reviews.Add(review);
                    return context.SaveChanges() > 0;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Ошибка при добавлении отзыва: {ex.Message}");
                    return false;
                }
            }
        }

        public static int GetGameReviewsCount(int gameId)
        {
            using (var context = new GameShopContext())
            {
                return context.Reviews
                    .AsNoTracking()
                    .Count(r => r.GameID == gameId);
            }
        }

        public static List<Game> GetGamesByGenre(int genreId)
        {
            using (var context = new GameShopContext())
            {
                return context.GameGenres
                    .AsNoTracking()
                    .Where(gg => gg.GenreID == genreId)
                    .Select(gg => gg.Game)
                    .ToList();
            }
        }

        public static List<Game> GetGamesByDeveloper(int developerId)
        {
            using (var context = new GameShopContext())
            {
                return context.GameDevelopers
                    .AsNoTracking()
                    .Where(gd => gd.DeveloperID == developerId)
                    .Select(gd => gd.Game)
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

        public static bool UpdateGameRating(int gameId, double newRating)
        {
            using (var context = new GameShopContext())
            {
                try
                {
                    var game = context.Games.Find(gameId);
                    if (game == null) return false;

                    game.Rating = (int)Math.Round(newRating, 1);
                    return context.SaveChanges() > 0;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Ошибка при обновлении рейтинга игры: {ex.Message}");
                    return false;
                }
            }
        }
        public static bool UpdateGameRating(int gameId, int newRating)
        {
            using (var context = new GameShopContext())
            {
                try
                {
                    var game = context.Games.Find(gameId);
                    if (game == null) return false;

                    game.Rating = newRating;
                    return context.SaveChanges() > 0;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Ошибка при обновлении рейтинга игры: {ex.Message}");
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