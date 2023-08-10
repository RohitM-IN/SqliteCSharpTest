using DBLayer;
using System.Data.SQLite;
using static DBLayer.Connection;

namespace SqliteTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Connection connection = new Connection();

            SQLiteConnection dbConnection = connection.OpenConnection();
            connection.InitDatabase();


            // Example: Insert data into the Users table
            string insertQuery = "INSERT INTO Users (Username, Email) VALUES (@Username, @Email);";
            using (SQLiteCommand command = new SQLiteCommand(insertQuery, dbConnection))
            {
                command.Parameters.AddWithValue("@Username", "exampleUser");
                command.Parameters.AddWithValue("@Email", "user@example.com");
                command.ExecuteNonQuery();
            }

            // Close the connection when done

            List<User> users = connection.GetAllUsers();
            foreach (User user in users)
            {
                Console.WriteLine($"ID: {user.Id}, Username: {user.Username}, Email: {user.Email}");
            }


            connection.CloseConnection(dbConnection);

            Console.ReadLine();
        }
    }
}