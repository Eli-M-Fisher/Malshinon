namespace MalshinonApp.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string SecretCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}