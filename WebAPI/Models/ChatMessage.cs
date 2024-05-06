namespace PasswordGameWebAPI.Models
{
    public class ChatMessage
    {
        public string Sender { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public ChatMessage(string sender, string text)
        {
            Sender = sender;
            Text = text;
        }
    }
}
