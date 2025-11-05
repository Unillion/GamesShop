using GamesShop.content.game;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace GamesShop.content.db
{
    public class GameDatabseManager
    {
        private static string connectionString = @"Server=MISHA1\SQLEXPRESS01;Database=gameshopdb;Trusted_Connection=True;TrustServerCertificate=True;";

        public static List<Game> GetAllGames()
        {
            return ExecuteQuery("SELECT ID, Title, Description, Genre, Price, ReleaseDate, Rating, Logo FROM Games");
        }

        public static Game GetGameById(int id)
        {
            var games = ExecuteQuery(
                "SELECT ID, Title, Description, Genre, Price, ReleaseDate, Rating, Logo FROM Games WHERE ID = @ID",
                new SqlParameter("@ID", id)
            );
            return games.Count > 0 ? games[0] : null;
        }

        public static List<Game> GetGamesByGenre(string genre)
        {
            return ExecuteQuery(
                "SELECT ID, Title, Description, Genre, Price, ReleaseDate, Rating, Logo FROM Games WHERE Genre = @Genre",
                new SqlParameter("@Genre", genre)
            );
        }

        public static List<Game> SearchGames(string searchTerm)
        {
            return ExecuteQuery(
                "SELECT ID, Title, Description, Genre, Price, ReleaseDate, Rating, Logo FROM Games WHERE Title LIKE @SearchTerm OR Description LIKE @SearchTerm",
                new SqlParameter("@SearchTerm", $"%{searchTerm}%")
            );
        }

        public static bool AddGame(Game game)
        {
            return ExecuteNonQuery(
                @"INSERT INTO Games (Title, Description, Genre, Price, ReleaseDate, Rating, Logo) 
                VALUES (@Title, @Description, @Genre, @Price, @ReleaseDate, @Rating, @Logo)",
                CreateGameParameters(game)
            );
        }

        public static bool UpdateGame(Game game)
        {
            return ExecuteNonQuery(
                @"UPDATE Games SET 
                Title = @Title, Description = @Description, Genre = @Genre, 
                Price = @Price, ReleaseDate = @ReleaseDate, Rating = @Rating, Logo = @Logo 
                WHERE ID = @ID",
                CreateGameParameters(game, true)
            );
        }

        public static bool DeleteGame(int id)
        {
            return ExecuteNonQuery(
                "DELETE FROM Games WHERE ID = @ID",
                new SqlParameter("@ID", id)
            );
        }

        // Вспомогательные методы

        public static List<Game> ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            var games = new List<Game>();

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        games.Add(CreateGameFromReader(reader));
                    }
                }
            }

            return games;
        }

        private static bool ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        private static Game CreateGameFromReader(SqlDataReader reader)
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

        private static SqlParameter[] CreateGameParameters(Game game, bool includeId = false)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Title", game.Title),
                new SqlParameter("@Description", game.Description),
                new SqlParameter("@Genre", game.Genre),
                new SqlParameter("@Price", game.Price),
                new SqlParameter("@ReleaseDate", game.ReleaseDate),
                new SqlParameter("@Rating", game.Rating),
                new SqlParameter("@Logo", game.Logo ?? (object)DBNull.Value)
            };

            if (includeId)
            {
                parameters.Add(new SqlParameter("@ID", game.ID));
            }

            return parameters.ToArray();
        }
    }
}