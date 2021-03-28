using System;
using System.Net.Http;
using System.Threading.Tasks;
using Beedux.Chat.App.Proxy;
using Beedux.Chat.App.State;
using Beedux.Core;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Beedux.Chat.App
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddReduxStore<RootState, IAction>(new RootState(), Reducers.RootReducer);

            builder.Services.AddScoped(sp => new ChatProxy(
                "https://localhost:5011/hubs/chat",
                sp.GetRequiredService<Store<RootState, IAction>>()));

            await builder.Build().RunAsync();
        }
    }
}
