using System;

namespace Meeteor.Core.Models
{
    public class ChatMessage
    {
        public string User { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTimeOffset Timestamp { get; set; }
    }
}
