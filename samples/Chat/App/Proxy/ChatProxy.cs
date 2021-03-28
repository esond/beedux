using System;
using System.Threading.Tasks;
using Beedux.Chat.App.State;
using Beedux.Chat.Core.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace Beedux.Chat.App.Proxy
{
    public class ChatProxy : IAsyncDisposable
    {
        private readonly Store<RootState, IAction> _store;
        private readonly Dispatcher<IAction> _dispatcher;

        private readonly HubConnection _connection;

        public ChatProxy(string hubUrl, Store<RootState, IAction> store)
        {
            _store = store;
            //_dispatcher = dispatcher;

            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();
        }

        public async Task ConnectAsync()
        {
            if (IsConnected)
                return;

            // todo: options configure method for actions
            _connection.On<ChatMessage>(Methods.Chat.ReceiveMessage,
                x => _store.Dispatch(new Actions.Chat.ReceiveMessage(x)));

            await _connection.StartAsync();
        }

        public bool IsConnected => _connection.State == HubConnectionState.Connected;

        public Task SendChatMessageAsync(string message)
        {
            return _connection.SendAsync(Methods.Chat.SendMessage, message);
        }

        #region Implementation of IAsyncDisposable

        public async ValueTask DisposeAsync()
        {
            await _connection.DisposeAsync();
        }

        #endregion
    }
}
