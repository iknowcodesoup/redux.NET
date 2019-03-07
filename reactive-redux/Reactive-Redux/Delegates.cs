namespace Redux
{
  using System;
  using System.Threading.Tasks;

  public delegate object Dispatcher(object action);

  public delegate Func<Dispatcher, Dispatcher> Middleware<TState>(IStore<TState> store);

  public delegate TState Reducer<TState>(TState previousState, object action);

  public delegate Task AsyncThunk<in TState>(Dispatcher dispatcher, TState getState);

  public delegate void Thunk<in TState>(Dispatcher dispatcher, TState getState);
}