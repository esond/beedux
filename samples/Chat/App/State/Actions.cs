using Beedux.Chat.Core.Models;

namespace Beedux.Chat.App.State
{
    public static class Actions
    {
        public static class Chat
        {
            public class SendMessage : IAction
            {
                public SendMessage(string content)
                {
                    Content = content;
                }

                public string Content { get; }
            }

            public class ReceiveMessage : IAction
            {
                public ReceiveMessage(ChatMessage message)
                {
                    ChatMessage = message;
                }

                public ChatMessage ChatMessage { get; }
            }
        }
    }
}
