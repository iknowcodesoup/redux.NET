namespace Redux
{
  using System;

  public interface IStore<TState>
  {
    object Dispatch(object action);

    TState CurrentState { get; }

    event Action StateChanged;

    object InnerDispatch(object action);
  }

  public class Store<TState> : IStore<TState>
  {
    private readonly object _syncRoot = new object();
    protected Dispatcher Dispatcher;
    private readonly Reducer<TState> _reducer;
    private Action _stateChanged;

    public TState CurrentState { get; private set; }

    public Store(
      Reducer<TState> reducer,
      TState initialState = default(TState),
      params Middleware<TState>[] middlewares)
    {
      _reducer = reducer;
      Dispatcher = ApplyMiddlewares(middlewares);
      CurrentState = initialState;
    }

    public event Action StateChanged
    {
      add
      {
        value();
        _stateChanged += value;
      }
      remove { _stateChanged -= value; }
    }

    public object Dispatch(object action)
    {
      return Dispatcher(action);
    }

    public virtual Dispatcher ApplyMiddlewares(params Middleware<TState>[] middlewares)
    {
      Dispatcher dispatcher = InnerDispatch;

      foreach (var middleware in middlewares)
      {
        dispatcher = middleware(this)(dispatcher);
      }

      return dispatcher;
    }

    public object InnerDispatch(object action)
    {
      lock (_syncRoot)
      {
        CurrentState = _reducer(CurrentState, action);
      }

      _stateChanged?.Invoke();

      return action;
    }
  }
}