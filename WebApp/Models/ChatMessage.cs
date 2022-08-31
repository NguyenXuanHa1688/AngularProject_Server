namespace WebApp.Models
{
    public class ChatMessage
    {
        public string user { get; set; }
        public string text { get; set; }
        public string connectionId { get; set; }
        public DateTime dateTime { get; set; }
    }
}
