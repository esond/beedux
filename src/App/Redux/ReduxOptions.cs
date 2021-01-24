using System;
using System.Text.Json;

namespace Beedux.App.Redux
{
    public class ReduxOptions<TState>
    {
        public ReduxOptions()
        {
            // Defaults
            StateSerializer = state => JsonSerializer.Serialize(state);
            StateDeserializer = s => JsonSerializer.Deserialize<TState>(s) ?? throw new ArgumentNullException();
        }

        public Reducer<TState, NewLocationAction> LocationReducer { get; set; }
        public Func<TState, string> GetLocation { get; set; }
        public Func<TState, string> StateSerializer { get; set; }
        public Func<string, TState> StateDeserializer { get; set; }
    }
}
