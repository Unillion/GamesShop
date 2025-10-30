using GamesShop.content.user;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.content.db
{
    public class UserDatabaseManager
    {
        private static string connectionString =
                @"Server=MISHA1\SQLEXPRESS01;Database=gameshopdb;Trusted_Connection=True;TrustServerCertificate=True;";


        public static bool AddUser(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Хешируем пароль
                    string passwordHash = HashPassword(user.Password);

                    string query = "INSERT INTO Users (Username, Email, PasswordHash) " +
                                   "VALUES (@Username, @Email, @PasswordHash)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", user.UserName);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при регистрации: {ex.Message}");
                return false;
            }
        }

        public static bool ValidateUserByUsername(string username, string password)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT PasswordHash FROM Users WHERE Username = @Username";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);

                        var result = cmd.ExecuteScalar();
                        if (result == null)
                            return false;

                        string storedHash = result.ToString();
                        string enteredHash = HashPassword(password);

                        return storedHash == enteredHash;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при входе: {ex.Message}");
                return false;
            }
        }

        public static decimal GetUserBalance(string username, string currencyCode = "USD")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT b.Amount
                        FROM Balances b
                        JOIN bill bl ON b.BillID = bl.ID
                        JOIN Users u ON bl.UserID = u.ID
                        WHERE u.Username = @Username AND b.CurrencyCode = @CurrencyCode";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@CurrencyCode", currencyCode);

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                            return Convert.ToDecimal(result);

                        return 0m;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при получении баланса: {ex.Message}");
                return 0m;
            }
        }

        public static bool UpdateUserBalance(string username, string currencyCode, decimal newAmount)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        UPDATE b
                        SET b.Amount = @Amount
                        FROM Balances b
                        JOIN bill bl ON b.BillID = bl.ID
                        JOIN Users u ON bl.UserID = u.ID
                        WHERE u.Username = @Username AND b.CurrencyCode = @CurrencyCode";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@CurrencyCode", currencyCode);
                        cmd.Parameters.AddWithValue("@Amount", newAmount);

                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при обновлении баланса: {ex.Message}");
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
    }
}
