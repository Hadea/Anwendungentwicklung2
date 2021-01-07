using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Memory
{
    public class HighScoreStorageSqlite : IHighScoreStorage
    {
        /// <summary>
        /// Adds a score entry to the database
        /// </summary>
        /// <param name="Tiles">Number of tiles of this game</param>
        /// <param name="totalMilliseconds">Elapsed time for this game in milliseconds</param>
        /// <param name="PlayerName">Name of the player</param>
        public void AddEntryToDatabase(int Tiles, double totalMilliseconds, string PlayerName)
        {
            // entweder builder nutzen oder auf https://www.connectionstrings.com/ nachschauen
            SQLiteConnectionStringBuilder builder = new();
            builder.DataSource = "config.db";
            builder.Version = 3;
            builder.FailIfMissing = true;

            using SQLiteConnection connection = new SQLiteConnection(builder.ToString()); // using gilt bis zum ende des scopes
            connection.Open();

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "insert into Scores (Name, SolveTime, TileNumber) values (@name, @time, @number);";
            command.Parameters.AddWithValue("name", PlayerName);
            command.Parameters.AddWithValue("time", totalMilliseconds);
            command.Parameters.AddWithValue("number", Tiles);

            if (command.ExecuteNonQuery() == 0)
                throw new Exception();
        }

        /// <summary>
        /// Reads the top 10 fastest games with identical tile count from database
        /// </summary>
        /// <param name="Tiles">Number of tiles of this game</param>
        /// <returns>Highscorelist ordered by fastest first with a maximum of 10 entries</returns>
        public List<Score> ReadHighscoreFromDatabase(int Tiles)
        {
            // entweder builder nutzen oder auf https://www.connectionstrings.com/ nachschauen
            SQLiteConnectionStringBuilder builder = new();
            builder.DataSource = "config.db";
            builder.Version = 3;
            builder.FailIfMissing = true; // exception wenn die Datenbank-Datei nicht existiert

            List<Score> highscore = new();

            using (SQLiteConnection connection = new SQLiteConnection(builder.ToString()))
            {
                connection.Open();

                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = "select Name, SolveTime, row_number() over ( order by SolveTime asc ) rank from Scores where TileNumber = @tiles limit 10;";
                command.Parameters.AddWithValue("tiles", Tiles);
                using var reader = command.ExecuteReader(); // using gilt bis zum ende des scope
                while (reader.Read())
                {
                    Score temp = new();
                    temp.Name = reader.GetString(0);
                    temp.Time = reader.GetDouble(1).ToString("N3");
                    temp.Rank = reader.GetInt32(2);
                    highscore.Add(temp);
                }
            }
            return highscore;
        }
    }
}
