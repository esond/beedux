namespace Meeteor.App.Redux
{
    public delegate void Dispatcher<in TAction>(TAction action);

    public delegate TState Reducer<TState, in TAction>(TState previousState, TAction action);
}
