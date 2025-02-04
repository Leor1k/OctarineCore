using System;

namespace Octarine_Core.Models
{
    public class MessagesDTO
    {
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; }
    }
}
