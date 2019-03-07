namespace Redux
{
  using System;
  using System.Reactive.Linq;
  using System.Threading.Tasks;

  public static class Extensions
  {
    public static IObservable<T> ObserveState<T>(this IStore<T> store)
    {
      return Observable
        .FromEvent(
          h => store.StateChanged += h,
          h => store.StateChanged -= h)
        .Select(_ => store.CurrentState);
    }

    public static Task DispatchAsync<TState>(this IStore<TState> store, AsyncThunk<TState> asyncThunk)
    {
      return asyncThunk(store.Dispatch, store.CurrentState);
    }

    public static void Dispatch<TState>(this IStore<TState> store, Thunk<TState> thunk)
    {
      thunk(store.Dispatch, store.CurrentState);
    }
  }
}