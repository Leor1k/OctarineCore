using System;

namespace Octarine_Core.Models
{
    public class MessagesDTO
    {
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
