using System.Threading.Tasks;
using Meeteor.App.Proxy;
using Meeteor.App.Redux;

namespace Meeteor.App.State
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
