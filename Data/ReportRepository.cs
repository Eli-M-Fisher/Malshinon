using MySql.Data.MySqlClient;
using MalshinonApp.Models;

namespace MalshinonApp.Data
{
    public class ReportRepository : IReportRepository
    {
        private readonly MySqlConnection _connection;

        public ReportRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public void Add(Report report)
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();

            string query = "INSERT INTO reports (reporter_id, target_id, report_text, timestamp) " +
                           "VALUES (@reporterId, @targetId, @text, @timestamp)";
            using var command = new MySqlCommand(query, _connection);

            command.Parameters.AddWithValue("@reporterId", report.ReporterId);
            command.Parameters.AddWithValue("@targetId", report.TargetId);
            command.Parameters.AddWithValue("@text", report.ReportText);
            command.Parameters.AddWithValue("@timestamp", report.Timestamp);

            command.ExecuteNonQuery();
            report.Id = (int)command.LastInsertedId;
            Console.WriteLine($"[DEBUG] Inserted report: ReporterId={report.ReporterId}, TargetId={report.TargetId}, Id={report.Id}");
        }

        public Report? GetById(int id)
        {
            var connection = _connection;
            string query = "SELECT * FROM reports WHERE id = @id";
            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return ReadReport(reader);
            }
            return null;
        }

        public List<Report> GetReportsByTargetId(int targetId)
        {
            var reports = new List<Report>();
            var connection = _connection;
            string query = "SELECT * FROM reports WHERE target_id = @targetId";
            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@targetId", targetId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                reports.Add(ReadReport(reader));
            }
            return reports;
        }

        public List<Report> GetReportsByReporterId(int reporterId)
        {
            var reports = new List<Report>();
            var connection = _connection;
            string query = "SELECT * FROM reports WHERE reporter_id = @reporterId";
            using var command = new MySqlCommand(query, _connection);
            command.Parameters.AddWithValue("@reporterId", reporterId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                reports.Add(ReadReport(reader));
            }
            return reports;
        }

        public List<Report> GetAll()
        {
            var reports = new List<Report>();
            var connection = _connection;
            string query = "SELECT * FROM reports";
            using var command = new MySqlCommand(query, _connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                reports.Add(ReadReport(reader));
            }
            return reports;
        }

        public List<Report> GetAllReports()
        {
            return GetAll();
        }

        private Report ReadReport(MySqlDataReader reader)
        {
            return new Report
            {
                Id = reader.GetInt32("id"),
                ReporterId = reader.GetInt32("reporter_id"),
                TargetId = reader.GetInt32("target_id"),
                ReportText = reader.GetString("report_text"),
                Timestamp = reader.GetDateTime("timestamp")
            };
        }
    }
}