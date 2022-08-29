namespace WebApp.Models
{
    public class LogActivities
    {
        public int Id { get; set; }
        public string User { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public string Action { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
