using MySql.Data.MySqlClient;
using MalshinonApp.Models;

namespace MalshinonApp.Data
{
    public class AlertRepository : IAlertRepository
    {
        public void Add(Alert alert)
        {
            using var connection = DbConnectionHelper.GetConnection();
            string query = "INSERT INTO alerts (target_id, alert_time_window_start, alert_time_window_end, reason, created_at) " +
                           "VALUES (@targetId, @start, @end, @reason, @createdAt)";
            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@targetId", alert.TargetId);
            command.Parameters.AddWithValue("@start", alert.AlertTimeWindowStart);
            command.Parameters.AddWithValue("@end", alert.AlertTimeWindowEnd);
            command.Parameters.AddWithValue("@reason", alert.Reason);
            command.Parameters.AddWithValue("@createdAt", alert.CreatedAt);

            command.ExecuteNonQuery();
            alert.Id = (int)command.LastInsertedId;
        }

        public List<Alert> GetAlertsByTargetId(int targetId)
        {
            var alerts = new List<Alert>();
            using var connection = DbConnectionHelper.GetConnection();
            string query = "SELECT * FROM alerts WHERE target_id = @targetId";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@targetId", targetId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                alerts.Add(ReadAlert(reader));
            }
            return alerts;
        }

        public List<Alert> GetAll()
        {
            var alerts = new List<Alert>();
            using var connection = DbConnectionHelper.GetConnection();
            string query = "SELECT * FROM alerts";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                alerts.Add(ReadAlert(reader));
            }
            return alerts;
        }

        private Alert ReadAlert(MySqlDataReader reader)
        {
            return new Alert
            {
                Id = reader.GetInt32("id"),
                TargetId = reader.GetInt32("target_id"),
                AlertTimeWindowStart = reader.GetDateTime("alert_time_window_start"),
                AlertTimeWindowEnd = reader.GetDateTime("alert_time_window_end"),
                Reason = reader.GetString("reason"),
                CreatedAt = reader.GetDateTime("created_at")
            };
        }
    }
}