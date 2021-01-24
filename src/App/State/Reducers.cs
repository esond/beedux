using System.Collections.Generic;
using Meeteor.App.Redux;
using Meeteor.Core.Models;

namespace Meeteor.App.State
{
    public class Reducers
    {
        public static RootState RootReducer(RootState state, IAction action)
        {
            return new RootState
            {
                ChatMessages = ChatReducer(state, action)
            };
        }

        public static IEnumerable<ChatMessage> ChatReducer(RootState state, IAction action)
        {
            return action switch
            {
                Actions.Chat.ReceiveMessage a => new List<ChatMessage>(state.ChatMessages)
                {
                    a.ChatMessage
                },
                _ => state.ChatMessages
            };
        }
    }
}
