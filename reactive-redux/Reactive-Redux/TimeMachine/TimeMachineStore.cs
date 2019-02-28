namespace Redux.TimeMachine
{
  /// Todo : Refactor to use StoreEnhancer
  public class TimeMachineStore<TState> : Store<TimeMachineState>, IStore<TState>
  {
    public TimeMachineStore(Reducer<TState> reducer, TState initialState = default(TState), params Middleware<TState>[] middlewares)
        : base(new TimeMachineReducer((state, action) => reducer((TState)state, action)).Execute, new TimeMachineState(initialState))
    {
      Middleware<TimeMachineState> middlewareWrapper()
      {
        return store =>
        {
          return next => action =>
          {
            var result = next(action);

            Dispatcher dispatcher = store.InnerDispatch;

            foreach (var middleware in middlewares)
            {
              dispatcher = middleware(this)(dispatcher);
            }

            return result;
          };
        };
      };

      base.ApplyMiddlewares(middlewareWrapper());
    }

    public new TState CurrentState => Unlift(base.CurrentState);

    private TState Unlift(TimeMachineState state)
    {
      return (TState)state.States[state.Position];
    }
  }
}