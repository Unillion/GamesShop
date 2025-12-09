
using GamesShop.content.models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace GamesShop.content.db
{
    public class UserDatabaseManager
    {
        public static bool AddUser(User user)
        {
            using (var context = new GameShopContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        user.PasswordHash = HashPassword(user.Password);
                        user.Password = null;

                        user.Balance = 0;

                        context.Users.Add(user);
                        context.SaveChanges();

                        var cart = new Cart { UserID = user.ID };
                        context.Carts.Add(cart);

                        var library = new Library { UserID = user.ID };
                        context.Libraries.Add(library);

                        var UserStatitics = new UserStatistics { UserID = user.ID };
                        context.UserStatistics.Add(UserStatitics);

                        context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Ошибка: {ex.Message}\nInner: {ex.InnerException?.Message}");
                        return false;
                    }
                }
            }
        }

        public static List<Game> GetUserCart(string username)
        {
            using (var context = new GameShopContext())
            {
                return context.Users
                    .Where(u => u.Username == username)
                    .SelectMany(u => u.Cart.CartItems)
                    .Select(ci => ci.Game)
                    .ToList();
            }
        }

        public static bool isUserExist(string username)
        {
            using (var context = new GameShopContext())
            {
                return context.Users
                    .Any(u => u.Username == username);
            }
        }

        public static bool isMailExist(string mail)
        {
            using (var context = new GameShopContext())
            {
                return context.Users
                    .Any(u => u.Email == mail);
            }
        }

        public static List<Game> GetUserLibrary(string username)
        {
            using (var context = new GameShopContext())
            {
                return context.Users
                    .Where(u => u.Username == username)
                    .SelectMany(u => u.Library.LibraryItems)
                    .Select(ci => ci.Game)
                    .ToList();
            }
        }

        public static decimal GetUserBalance(string username)
        {
            using (var context = new GameShopContext())
            {
                return context.Users
                    .Where(u => u.Username == username)
                    .Select(u => u.Balance)
                    .FirstOrDefault();
            }
        }

        public static bool IsGameInCart(string username, int gameId)
        {
            var cartGames = GetUserCart(username);
            return cartGames.Any(game => game.ID == gameId);
        }

        public static bool AddGameToCart(string username, int gameId)
        {
            using (var context = new GameShopContext())
            {
                var user = context.Users
                    .Include(u => u.Cart)
                    .ThenInclude(c => c.CartItems)
                    .FirstOrDefault(u => u.Username == username);

                if (user?.Cart == null) return false;

                if (!user.Cart.CartItems.Any(ci => ci.GameID == gameId))
                {
                    user.Cart.CartItems.Add(new CartItem
                    {
                        GameID = gameId
                    });
                    return context.SaveChanges() > 0;
                }

                return true;
            }
        }

        public static bool AddGameToLibrary(string username, int gameId)
        {
            using (var context = new GameShopContext())
            {
                var user = context.Users
                    .Include(u => u.Library)
                    .ThenInclude(c => c.LibraryItems)
                    .FirstOrDefault(u => u.Username == username);

                if (user?.Library == null) return false;

                if (!user.Library.LibraryItems.Any(ci => ci.GameID == gameId))
                {
                    user.Library.LibraryItems.Add(new LibraryItem
                    {
                        GameID = gameId
                    });
                    return context.SaveChanges() > 0;
                }

                return true;
            }
        }

        public static bool IsGamePurchased(string username, int gameId)
        {
            using (var context = new GameShopContext())
            {
                return context.LibraryItems
                    .Any(l => l.Library.User.Username == username && l.GameID == gameId);
            }
        }

        public static bool RemoveGameFromCart(string username, int gameId)
        {
            using (var context = new GameShopContext())
            {
                var cartItem = context.CartItems
                    .Include(ci => ci.Cart)
                    .ThenInclude(c => c.User)
                    .FirstOrDefault(ci => ci.Cart.User.Username == username && ci.GameID == gameId);

                if (cartItem != null)
                {
                    context.CartItems.Remove(cartItem);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        public static bool ValidateUserByUsername(string username, string password)
        {
            using (var context = new GameShopContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == username);
                if (user == null) return false;

                return user.PasswordHash == HashPassword(password);
            }
        }

        public static int GetUserId(string username)
        {
            using (var context = new GameShopContext())
            {
                var user = context.Users
                    .FirstOrDefault(u => u.Username == username);

                return user?.ID ?? -1;
            }
        }

        public static bool AddToUserBalance(string username, decimal amount)
        {
            using (var context = new GameShopContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    user.Balance += amount;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public static bool removeFromUserBalance(string username, decimal amount)
        {
            using (var context = new GameShopContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    user.Balance = user.Balance - amount;

                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public static bool UserHasReviewForGame(string username, int gameId)
        {
            using (var context = new GameShopContext())
            {
                return context.Reviews
                    .Any(r => r.User.Username == username && r.GameID == gameId);
            }
        }

        public static User GetUserById(int userId, bool includeRelated = false)
        {
            using (var context = new GameShopContext())
            {
                var query = context.Users.AsQueryable();

                if (includeRelated)
                {
                    query = query
                        .Include(u => u.Cart)
                            .ThenInclude(c => c.CartItems)
                        .Include(u => u.Library)
                            .ThenInclude(l => l.LibraryItems)
                        .Include(u => u.Statistics)
                        .Include(u => u.Reviews);
                }

                return query
                    .AsNoTracking()
                    .FirstOrDefault(u => u.ID == userId);
            }
        }

        public static UserStatistics GetUserStatistics(int userId)
        {
            using (var context = new GameShopContext())
            {
                return context.UserStatistics
                    .AsNoTracking()
                    .FirstOrDefault(us => us.UserID == userId);
            }
        }

        public static bool UpdateMultipleStats(int userId, decimal moneySpent = 0, int gamesPurchased = 0, int reviewsWritten = 0, decimal income = 0, int nameChanges = 0, int passwordChanges = 0)
        {
            using var context = new GameShopContext();
            var stats = context.UserStatistics.FirstOrDefault(us => us.UserID == userId);
            if (stats == null) return false;

            if (moneySpent > 0) stats.TotalMoneySpent += moneySpent;
            if (gamesPurchased > 0) stats.TotalGamesPurchased += gamesPurchased;
            if (reviewsWritten > 0) stats.ReviewsWritten += reviewsWritten;
            if (income > 0) stats.TotalIncomeAmount += income;
            if (nameChanges > 0) stats.NameChanges += nameChanges;
            if (passwordChanges > 0) stats.PasswordChanges += passwordChanges;

            return context.SaveChanges() > 0;
        }

        public static bool UpdateUser(int userId, string newUsername = null, string newPassword = null)
        {
            using var context = new GameShopContext();
            var user = context.Users.FirstOrDefault(u => u.ID == userId);
            if (user == null) return false;

            if (!string.IsNullOrWhiteSpace(newUsername)) user.Username = newUsername;
            if (!string.IsNullOrWhiteSpace(newPassword)) user.PasswordHash = HashPassword(newPassword);
            return context.SaveChanges() > 0;
        }
    }
}