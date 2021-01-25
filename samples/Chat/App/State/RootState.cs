using System.Collections.Generic;
using System.Linq;
using Beedux.Chat.Core.Models;

namespace Beedux.Chat.App.State
{
    public class RootState
    {
        public IEnumerable<ChatMessage> ChatMessages { get; set; } = Enumerable.Empty<ChatMessage>();
    }
}
