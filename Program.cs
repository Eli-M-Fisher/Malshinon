using MalshinonApp.Data;
using MySql.Data.MySqlClient;

Console.WriteLine("Connecting to DB...");

try
{
    using var connection = DbConnectionHelper.GetConnection();
    string query = "SELECT id, full_name, secret_code FROM people";

    using var command = new MySqlCommand(query, connection);
    using var reader = command.ExecuteReader();

    while (reader.Read())
    {
        int id = reader.GetInt32("id");
        string name = reader.GetString("full_name");
        string code = reader.GetString("secret_code");

        Console.WriteLine($"#{id} {name} ({code})");
    }
}
catch (Exception ex)
{
    Console.WriteLine("Connection failed:");
    Console.WriteLine(ex.Message);
}