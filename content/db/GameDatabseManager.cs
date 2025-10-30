using GamesShop.content.game;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.content.db
{
    public class GameDatabseManager
    {
        private static string connectionString = @"Server=MISHA1\SQLEXPRESS01;Database=gameshopdb;Trusted_Connection=True;TrustServerCertificate=True;";

        public static List<Game> GetAllGames()
        {
            var games = new List<Game>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ID, Title, Description, Genre, Price, ReleaseDate, Rating, Logo FROM Games";

                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        games.Add(new Game
                        {
                            ID = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            Genre = reader.GetString(3),
                            Price = reader.GetDecimal(4),
                            ReleaseDate = reader.GetDateTime(5),
                            Rating = reader.GetInt32(6),
                            Logo = reader.GetString(7)
                        });
                    }
                }
            }

            return games;
        }

        public static Game GetGameById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ID, Title, Description, Genre, Price, ReleaseDate, Rating, Logo FROM Games WHERE ID = @ID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Game
                            {
                                ID = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.GetString(2),
                                Genre = reader.GetString(3),
                                Price = reader.GetDecimal(4),
                                ReleaseDate = reader.GetDateTime(5),
                                Rating = reader.GetInt32(6),
                                Logo = reader.GetString(7)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public static List<Game> GetGamesByGenre(string genre)
        {
            var games = new List<Game>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ID, Title, Description, Genre, Price, ReleaseDate, Rating, Logo FROM Games WHERE Genre = @Genre";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Genre", genre);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            games.Add(new Game
                            {
                                ID = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.GetString(2),
                                Genre = reader.GetString(3),
                                Price = reader.GetDecimal(4),
                                ReleaseDate = reader.GetDateTime(5),
                                Rating = reader.GetInt32(6),
                                Logo = reader.GetString(7)
                            });
                        }
                    }
                }
            }

            return games;
        }

        public static List<Game> SearchGames(string searchTerm)
        {
            var games = new List<Game>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ID, Title, Description, Genre, Price, ReleaseDate, Rating, Logo FROM Games WHERE Title LIKE @SearchTerm OR Description LIKE @SearchTerm";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            games.Add(new Game
                            {
                                ID = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.GetString(2),
                                Genre = reader.GetString(3),
                                Price = reader.GetDecimal(4),
                                ReleaseDate = reader.GetDateTime(5),
                                Rating = reader.GetInt32(6),
                                Logo = reader.GetString(7)
                            });
                        }
                    }
                }
            }

            return games;
        }

        public static bool AddGame(Game game)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO Games (Title, Description, Genre, Price, ReleaseDate, Rating, Logo) 
                               VALUES (@Title, @Description, @Genre, @Price, @ReleaseDate, @Rating, @Logo)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", game.Title);
                    command.Parameters.AddWithValue("@Description", game.Description);
                    command.Parameters.AddWithValue("@Genre", game.Genre);
                    command.Parameters.AddWithValue("@Price", game.Price);
                    command.Parameters.AddWithValue("@ReleaseDate", game.ReleaseDate);
                    command.Parameters.AddWithValue("@Rating", game.Rating);
                    command.Parameters.AddWithValue("@Logo", game.Logo ?? (object)DBNull.Value);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool UpdateGame(Game game)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"UPDATE Games SET 
                               Title = @Title, 
                               Description = @Description, 
                               Genre = @Genre, 
                               Price = @Price, 
                               ReleaseDate = @ReleaseDate, 
                               Rating = @Rating, 
                               Logo = @Logo 
                               WHERE ID = @ID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", game.ID);
                    command.Parameters.AddWithValue("@Title", game.Title);
                    command.Parameters.AddWithValue("@Description", game.Description);
                    command.Parameters.AddWithValue("@Genre", game.Genre);
                    command.Parameters.AddWithValue("@Price", game.Price);
                    command.Parameters.AddWithValue("@ReleaseDate", game.ReleaseDate);
                    command.Parameters.AddWithValue("@Rating", game.Rating);
                    command.Parameters.AddWithValue("@Logo", game.Logo ?? (object)DBNull.Value);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public static bool DeleteGame(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Games WHERE ID = @ID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
