namespace Redux
{
  using System;

  public delegate object Dispatcher(object action);

  public delegate Func<Dispatcher, Dispatcher> Middleware<TState>(IStore<TState> store);

  public delegate TState Reducer<TState>(TState previousState, object action);
}