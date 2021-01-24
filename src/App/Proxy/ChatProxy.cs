using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Meeteor.App.Redux;
using Meeteor.App.State;
using Meeteor.Core.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;

namespace Meeteor.App.Proxy
{
    public class ChatProxy : IAsyncDisposable
    {
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly Store<RootState, IAction> _store;
        private readonly Dispatcher<IAction> _dispatcher;

        private HubConnection _connection;

        public ChatProxy(IAccessTokenProvider accessTokenProvider,
            Store<RootState, IAction> store)
        {
            _accessTokenProvider = accessTokenProvider;
            _store = store;
            //_dispatcher = dispatcher;

            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:49153/hubs/chat", options => //todo: config
                {
                    options.AccessTokenProvider = async () =>
                    {
                        var tokenResult = await _accessTokenProvider.RequestAccessToken();

                        tokenResult.TryGetToken(out var accessToken);

                        return accessToken.Value;
                    };
                })
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
