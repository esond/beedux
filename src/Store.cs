using System;
using System.Collections.Generic;

//using Microsoft.AspNetCore.Mvc.Routing;

namespace Beedux
{
    public class Store<TState, TAction> : IDisposable
    {
        private readonly TState _initialState;
        private readonly ReduxOptions<TState> _options;
        private readonly Reducer<TState, TAction> _rootReducer;

        private readonly object _syncRoot = new object();

        //private readonly DevToolsInterop _devToolsInterop;
        //private UrlHelper _uriHelper;
        private string _currentLocation;
        private bool _timeTraveling;

        //public Store(TState initialState, Reducer<TState, TAction> rootReducer, ReduxOptions<TState> options, DevToolsInterop devToolsInterop)
        public Store(TState initialState, Reducer<TState, TAction> rootReducer, ReduxOptions<TState> options)
        {
            _initialState = initialState;
            _rootReducer = rootReducer;
            _options = options;
            //_devToolsInterop = devToolsInterop;

            State = initialState;

            //_devToolsInterop.Reset += OnDevToolsReset;
            //_devToolsInterop.TimeTravel += OnDevToolsTimeTravel;
            //_devToolsInterop.Log("initial", _options.StateSerializer(State));

            History = new List<HistoricEntry<TState, object>> { new HistoricEntry<TState, object>(State) };
        }

        public TState State { get; private set; }

        public IList<HistoricEntry<TState, object>> History { get; }

        //internal void Init(UrlHelper uriHelper)
        //{
        //    if (_uriHelper != null || uriHelper == null) return;

        //    lock (_syncRoot)
        //    {
        //        _uriHelper = uriHelper;
        //        _uriHelper. += OnLocationChanged;
        //    }

        //    //// TODO: Queue up any other actions, and let this apply to the initial state.
        //    DispatchLocation(new NewLocationAction { Location = _uriHelper.GetAbsoluteUri() });

        //    Console.WriteLine("Redux store initialized.");
        //}

        public void Dispose()
        {
            //if (_uriHelper != null)
            //    _uriHelper.OnLocationChanged -= OnLocationChanged;

            //_devToolsInterop.Reset -= OnDevToolsReset;
            //_devToolsInterop.TimeTravel -= OnDevToolsTimeTravel;
        }

        public event EventHandler Change;

        private void OnLocationChanged(object sender, string newAbsoluteUri)
        {
            if (_timeTraveling)
                return;

            if (newAbsoluteUri == _currentLocation)
                return;

            lock (_syncRoot)
            {
                _currentLocation = newAbsoluteUri;
            }

            DispatchLocation(new NewLocationAction { Location = newAbsoluteUri });
        }

        private void OnDevToolsReset(object sender, EventArgs e)
        {
            var state = _initialState;
            TimeTravel(state);
        }

        private void OnDevToolsTimeTravel(object sender, StringEventArgs e)
        {
            var state = _options.StateDeserializer(e.String);
            _timeTraveling = true;
            TimeTravel(state);
            _timeTraveling = false;
        }

        private void OnChange(EventArgs? e)
        {
            var handler = Change;
            handler?.Invoke(this, e);

            var getLocation = _options.GetLocation;

            if (getLocation == null)
                return;

            var newLocation = getLocation(State);

            if (newLocation == _currentLocation || newLocation == null)
                return;

            lock (_syncRoot)
            {
                _currentLocation = newLocation;
            }

            //_uriHelper.NavigateTo(newLocation);
        }

        public void Dispatch(TAction action)
        {
            lock (_syncRoot)
            {
                State = _rootReducer(State, action);
                //_devToolsInterop.Log(action.ToString(), _options.StateSerializer(State));
                History.Add(new HistoricEntry<TState, object>(State, action));
            }

            OnChange(null);
        }

        private void DispatchLocation(NewLocationAction locationAction)
        {
            var locationReducer = _options.LocationReducer;

            if (locationReducer == null && locationAction is TAction)
            {
                // Just use the RootReducer unless the user has configured a LocationReducer
                var genericAction = (TAction) (object) locationAction;
                Dispatch(genericAction);
            }

            if (locationReducer == null)
                return;

            lock (_syncRoot)
            {
                State = locationReducer(State, locationAction);
                //_devToolsInterop.Log(locationAction.ToString(), _options.StateSerializer(State));
                History.Add(new HistoricEntry<TState, object>(State, locationAction));
            }

            OnChange(null);
        }

        public void TimeTravel(TState state)
        {
            lock (_syncRoot)
            {
                State = state;
            }

            OnChange(null);
        }
    }
}
