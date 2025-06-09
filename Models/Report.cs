namespace MalshinonApp.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public int TargetId { get; set; }
        public string ReportText { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}