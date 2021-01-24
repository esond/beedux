using System;
using System.Net.Http;
using System.Threading.Tasks;
using Beedux.App.Proxy;
using Beedux.App.Redux;
using Beedux.App.State;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Beedux.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Auth0", options.ProviderOptions);
                options.ProviderOptions.ResponseType = "code";
            });

            builder.Services.AddReduxStore<RootState, IAction>(new RootState(), Reducers.RootReducer);

            builder.Services.AddScoped(sp => new ChatProxy(
                sp.GetRequiredService<IAccessTokenProvider>(),
                "https://localhost:49157/hubs/chat",
                sp.GetRequiredService<Store<RootState, IAction>>()));

            await builder.Build().RunAsync();
        }
    }
}
