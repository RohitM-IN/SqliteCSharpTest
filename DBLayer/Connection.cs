using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace DBLayer
{
    public class Connection
    {
        private string _connectionString;

        public Connection()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string folderPath = Path.Combine(documentsPath, "YourDatabaseFolder");
            Directory.CreateDirectory(folderPath);

            string databasePath = Path.Combine(folderPath, "YourDatabaseName.db");
            _connectionString = $"Data Source={databasePath};Version=3;";
        }

        public SQLiteConnection OpenConnection()
        {
            SQLiteConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public void CloseConnection(SQLiteConnection connection)
        {
            connection.Close();
        }

        public void InitDatabase()
        {
            using (SQLiteConnection connection = OpenConnection())
            {
                // Create database tables if they don't exist
                string createUserTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY,
                        Username TEXT NOT NULL,
                        Email TEXT NOT NULL
                    );";

                using (SQLiteCommand command = new SQLiteCommand(createUserTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Add more table creation queries here if needed
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (SQLiteConnection connection = OpenConnection())
            {
                string selectQuery = "SELECT Id, Username, Email FROM Users;";
                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string username = reader.GetString(1);
                        string email = reader.GetString(2);

                        users.Add(new User { Id = id, Username = username, Email = email });
                    }
                }
            }

            return users;
        }

        public class User
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
        }
    }
}