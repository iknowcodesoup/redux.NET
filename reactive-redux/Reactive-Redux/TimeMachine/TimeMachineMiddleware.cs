namespace Redux.TimeMachine
{
  using Redux;
  using System;

  public class TimeMachineMiddleware<TState> : IDisposable where TState : class, new()
  {
    private TimeMachineState<TState> timeMachineState = new TimeMachineState<TState>();

    public Middleware<TState> CreateMiddleware()
    {
      return store =>
      {
        return next => action =>
        {
          var result = next(action);

          timeMachineState = TimeMachineReducer.Execute(timeMachineState, store.CurrentState, action);

          switch (action)
          {
            case TimeMachineActions.UndoAction _:
            case TimeMachineActions.RedoAction _:
              store.ReplaceState(timeMachineState.States[timeMachineState.Position]);
              break;
          }

          return result;
        };
      };
    }

    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          timeMachineState.Dispose();
        }

        disposedValue = true;
      }
    }

    public void Dispose()
    {
      Dispose(true);
    }
  }
}
