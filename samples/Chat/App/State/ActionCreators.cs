using System.Threading.Tasks;
using Beedux.Chat.App.Proxy;
using Beedux.Core;

namespace Beedux.Chat.App.State
{
    public class ActionCreators
    {
        public class Chat
        {
            // TODO: weird to have to inject proxy here
            public static Task Send(Dispatcher<IAction> dispatch, ChatProxy proxy, string content)
            {
                dispatch(new Actions.Chat.SendMessage(content));

                return proxy.SendChatMessageAsync(content);
            }
        }
    }
}
