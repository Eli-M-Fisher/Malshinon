namespace MalshinonApp.Models
{
    public class Alert
    {
        public int Id { get; set; }
        public int TargetId { get; set; }
        public DateTime AlertTimeWindowStart { get; set; }
        public DateTime AlertTimeWindowEnd { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}