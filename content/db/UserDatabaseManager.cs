
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

                        context.Users.Add(user);
                        context.SaveChanges();

                        var cart = new Cart { UserID = user.ID };
                        context.Carts.Add(cart);

                        var library = new Library { UserID = user.ID };
                        context.Libraries.Add(library);

                        var bill = new Bill { UserID = user.ID };
                        context.Bills.Add(bill);
                        context.SaveChanges();

                        var balance = new Balance { BillID = bill.ID, CurrencyCode = "USD", Amount = 0 };
                        context.Balances.Add(balance);

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

        private static void CreateUserRelatedEntities(int userId)
        {
            using (var context = new GameShopContext())
            {
                var cart = new Cart { UserID = userId };
                context.Carts.Add(cart);

                var library = new Library { UserID = userId };
                context.Libraries.Add(library);

                var bill = new Bill { UserID = userId };
                context.Bills.Add(bill);
                context.SaveChanges();

                var balance = new Balance { BillID = bill.ID, CurrencyCode = "USD", Amount = 0 };
                context.Balances.Add(balance);

                context.SaveChanges();
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

        public static bool UserHasReviewForGame(string username, int gameId)
        {
            using (var context = new GameShopContext())
            {
                return context.Reviews
                    .Any(r => r.User.Username == username && r.GameID == gameId);
            }
        }
    }
}