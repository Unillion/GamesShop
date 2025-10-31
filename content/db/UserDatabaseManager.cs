using GamesShop.content.user;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace GamesShop.content.db
{
    public class UserDatabaseManager
    {
        private static string connectionString =
            @"Server=MISHA1\SQLEXPRESS01;Database=gameshopdb;Trusted_Connection=True;TrustServerCertificate=True;";

        public static bool AddUser(User user)
        {
            string query = @"
                INSERT INTO Users (Username, Email, PasswordHash) 
                VALUES (@Username, @Email, @PasswordHash);
        
                INSERT INTO Libraries (UserID) 
                VALUES (IDENT_CURRENT('Users'));
                
                INSERT INTO Carts (UserID) 
                VALUES (IDENT_CURRENT('Users'));

                INSERT INTO bill (UserID)
                VALUES (IDENT_CURRENT('Users'));";

            
            return ExecuteNonQuery(
                query,
                new SqlParameter("@Username", user.UserName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@PasswordHash", HashPassword(user.Password))
            );
        }

        public static bool ValidateUserByUsername(string username, string password)
        {
            var storedHash = ExecuteScalar<string>(
                "SELECT PasswordHash FROM Users WHERE Username = @Username",
                new SqlParameter("@Username", username)
            );

            return storedHash != null && storedHash == HashPassword(password);
        }

        public static decimal GetUserBalance(string username, string currencyCode = "USD")
        {
            return ExecuteScalar<decimal?>(
                @"SELECT b.Amount
                  FROM Balances b
                  JOIN bill bl ON b.BillID = bl.ID
                  JOIN Users u ON bl.UserID = u.ID
                  WHERE u.Username = @Username AND b.CurrencyCode = @CurrencyCode",
                new SqlParameter("@Username", username),
                new SqlParameter("@CurrencyCode", currencyCode)
            ) ?? 0m;
        }

        public static bool UpdateUserBalance(string username, string currencyCode, decimal newAmount)
        {
            return ExecuteNonQuery(
                @"UPDATE b
                  SET b.Amount = @Amount
                  FROM Balances b
                  JOIN bill bl ON b.BillID = bl.ID
                  JOIN Users u ON bl.UserID = u.ID
                  WHERE u.Username = @Username AND b.CurrencyCode = @CurrencyCode",
                new SqlParameter("@Username", username),
                new SqlParameter("@CurrencyCode", currencyCode),
                new SqlParameter("@Amount", newAmount)
            );
        }

        // Вспомогательные методы

        private static bool ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при выполнении запроса: {ex.Message}");
                return false;
            }
        }

        private static T ExecuteScalar<T>(string query, params SqlParameter[] parameters)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return result != null && result != DBNull.Value ? (T)Convert.ChangeType(result, typeof(T)) : default(T);
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при получении данных: {ex.Message}");
                return default(T);
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

        private static void ShowError(string message)
        {
            System.Windows.MessageBox.Show(message);
        }
    }
}