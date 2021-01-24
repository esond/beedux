using System.Collections.Generic;
using System.Linq;
using Beedux.Core.Models;

namespace Beedux.App.State
{
    public class RootState
    {
        public IEnumerable<ChatMessage> ChatMessages { get; set; } = Enumerable.Empty<ChatMessage>();
    }
}
