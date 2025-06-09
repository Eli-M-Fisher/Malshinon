using MySql.Data.MySqlClient;
using MalshinonApp.Models;

namespace MalshinonApp.Data
{
    public class ReportRepository : IReportRepository
    {
        public void Add(Report report)
        {
            using var connection = DbConnectionHelper.GetConnection();
            string query = "INSERT INTO reports (reporter_id, target_id, report_text, timestamp) " +
                           "VALUES (@reporterId, @targetId, @text, @timestamp)";
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@reporterId", report.ReporterId);
            command.Parameters.AddWithValue("@targetId", report.TargetId);
            command.Parameters.AddWithValue("@text", report.ReportText);
            command.Parameters.AddWithValue("@timestamp", report.Timestamp);

            command.ExecuteNonQuery();
            report.Id = (int)command.LastInsertedId;
        }

        public Report? GetById(int id)
        {
            using var connection = DbConnectionHelper.GetConnection();
            string query = "SELECT * FROM reports WHERE id = @id";
            using var command = new MySqlCommand(query, connection);
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
            using var connection = DbConnectionHelper.GetConnection();
            string query = "SELECT * FROM reports WHERE target_id = @targetId";
            using var command = new MySqlCommand(query, connection);
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
            using var connection = DbConnectionHelper.GetConnection();
            string query = "SELECT * FROM reports WHERE reporter_id = @reporterId";
            using var command = new MySqlCommand(query, connection);
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
            using var connection = DbConnectionHelper.GetConnection();
            string query = "SELECT * FROM reports";
            using var command = new MySqlCommand(query, connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                reports.Add(ReadReport(reader));
            }
            return reports;
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