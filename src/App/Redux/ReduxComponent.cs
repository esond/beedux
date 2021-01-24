using System;
using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Mvc.Routing;

namespace Beedux.App.Redux
{
    public class ReduxComponent<TState, TAction> : ComponentBase, IDisposable
    {
        [Inject] public Store<TState, TAction> Store { get; set; }
        //[Inject] private IUriHelper UriHelper { get; set; }

        public TState State => Store.State;

        public RenderFragment ReduxDevTools;

        public void Dispose()
        {
            Store.Change -= OnChangeHandler;
        }

        protected override void OnInitialized()
        {
            //Store.Init(new UrlHelper());
            Store.Change += OnChangeHandler;

            ReduxDevTools = builder =>
            {
                var seq = 0;
                //builder.OpenComponent<ReduxDevTools>(seq);
                builder.CloseComponent();
            };
        }

        private void OnChangeHandler(object sender, EventArgs e)
        {
            StateHasChanged();
        }

        public void Dispatch(TAction action)
        {
            Store.Dispatch(action);
        }
    }
}
