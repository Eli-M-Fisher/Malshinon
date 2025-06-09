using MySql.Data.MySqlClient;
using MalshinonApp.Models;

namespace MalshinonApp.Data
{
    public class PersonRepository : IPersonRepository
    {
        public void Add(Person person)
        {
            using var connection = DbConnectionHelper.GetConnection();
            string query = "INSERT INTO people (full_name, secret_code, created_at) VALUES (@name, @code, @createdAt)";
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@name", person.FullName);
            command.Parameters.AddWithValue("@code", person.SecretCode);
            command.Parameters.AddWithValue("@createdAt", person.CreatedAt);

            command.ExecuteNonQuery();
            person.Id = (int)command.LastInsertedId;
        }

        public Person? GetById(int id)
        {
            using var connection = DbConnectionHelper.GetConnection();
            string query = "SELECT * FROM people WHERE id = @id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return ReadPerson(reader);
            }
            return null;
        }

        public Person? GetBySecretCode(string code)
        {
            using var connection = DbConnectionHelper.GetConnection();
            string query = "SELECT * FROM people WHERE secret_code = @code";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@code", code);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return ReadPerson(reader);
            }
            return null;
        }

        public Person? GetByFullName(string name)
        {
            using var connection = DbConnectionHelper.GetConnection();
            string query = "SELECT * FROM people WHERE full_name = @name";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@name", name);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return ReadPerson(reader);
            }
            return null;
        }

        public List<Person> GetAll()
        {
            var people = new List<Person>();
            using var connection = DbConnectionHelper.GetConnection();
            string query = "SELECT * FROM people";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                people.Add(ReadPerson(reader));
            }
            return people;
        }

        private Person ReadPerson(MySqlDataReader reader)
        {
            return new Person
            {
                Id = reader.GetInt32("id"),
                FullName = reader.GetString("full_name"),
                SecretCode = reader.GetString("secret_code"),
                CreatedAt = reader.GetDateTime("created_at")
            };
        }

        public void Update(Person person)
        {
            using var connection = DbConnectionHelper.GetConnection();
            string query = "UPDATE people SET full_name = @name, secret_code = @code WHERE id = @id";
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@name", person.FullName);
            command.Parameters.AddWithValue("@code", person.SecretCode);
            command.Parameters.AddWithValue("@id", person.Id);

            command.ExecuteNonQuery();
        }
    }
}