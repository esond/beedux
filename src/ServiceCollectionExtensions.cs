using System;
using Microsoft.Extensions.DependencyInjection;

namespace Beedux
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddReduxStore<TState, TAction>(this IServiceCollection configure,
            TState initialState, Reducer<TState, TAction> rootReducer, Action<ReduxOptions<TState>>? options = null)
        {
            //configure.AddSingleton<DevToolsInterop>();
            var reduxOptions = new ReduxOptions<TState>();
            options?.Invoke(reduxOptions);
            configure.AddSingleton(sp => new Store<TState, TAction>(initialState, rootReducer, reduxOptions));
            return configure;
        }
    }
}
