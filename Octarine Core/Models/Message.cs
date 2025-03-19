using System;
using System.Text.Json.Serialization;

namespace Octarine_Core.Models
{
    public class Message
    {
        [JsonPropertyName("messageid")]
        public int MessageId { get; set; }

        [JsonPropertyName("chatid")]
        public int ChatId { get; set; }

        [JsonPropertyName("senderid")]
        public int SenderId {  get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
        //[JsonPropertyName("messagetype")]
        //public string MessageType {  get; set; }

        [JsonPropertyName("createdat")]
        public DateTime? CreatedAt { get; set; }
    }
}
