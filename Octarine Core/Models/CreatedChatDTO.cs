
using System.Text.Json.Serialization;

namespace Octarine_Core.Models
{
    public class CreatedChatDTO
    {
        [JsonPropertyName("chatId")]
        public int ChatId { get; set; }
        [JsonPropertyName("chattype")]
        public string ChatType { get; set; }
    }
}
