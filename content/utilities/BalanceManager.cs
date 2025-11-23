using GamesShop.content.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.content.utilities
{
    public class BalanceManager
    {
        public static event Action<string, decimal> UserBalanceChanged;

        public static void UpdateBalance(string username, decimal amount)
        {
            UserDatabaseManager.AddToUserBalance(username, amount);
            decimal newBalance = UserDatabaseManager.GetUserBalance(username);
            UserBalanceChanged?.Invoke(username, newBalance);
        }
    }
}
