using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace ChatCommon
{
    public class Message
    {
        public Command Command { get; set; }
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime TimeMessage { get; set; }
        public bool ReceivedStatus { get; set; } = false;
        public string SenderName { get; set; } = "User";

        public string? RecipientName { get; set; }

        public static Message? FromJson(string json)
        {
            return JsonSerializer.Deserialize<Message>(json);
        }
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        public override string ToString()
        {
            return $"[{TimeMessage}] {SenderName}: {Text}";
        }
    }
}
