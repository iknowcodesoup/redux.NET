namespace BurnCalc.State.Middleware
{
  using Redux;
  using Redux.TimeMachine;
  using System;

  public class TimeMachineMiddleware<TState> : IDisposable where TState : class, new()
  {
    public TimeMachineState<TState> timeMachineState = new TimeMachineState<TState>();

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
              store.ReplaceState(timeMachineState.States[timeMachineState.Position - 1]);
              break;
          }

          return result;
        };
      };
    }

    private void ClearState()
    {
      timeMachineState.States.Clear();
      timeMachineState.Actions.Clear();
    }

    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          ClearState();
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
