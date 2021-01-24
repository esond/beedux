using System.Collections.Generic;
using System.Linq;
using Meeteor.Core.Models;

namespace Meeteor.App.State
{
    public class RootState
    {
        public IEnumerable<ChatMessage> ChatMessages { get; set; } = Enumerable.Empty<ChatMessage>();
    }
}
