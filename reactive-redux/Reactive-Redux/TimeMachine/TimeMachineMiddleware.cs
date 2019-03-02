namespace BurnCalc.State.Middleware
{
  using Redux;
  using Redux.TimeMachine;
  using System;

  public class TimeMachineMiddleware<TState> : IDisposable where TState : class, new()
  {
    public static TimeMachineState timeMachineState = new TimeMachineState();

    public Middleware<TState> CreateMiddleware()
    {
      return store =>
      {
        return next => action =>
        {
          var result = next(action);

          timeMachineState = TimeMachineReducer.Execute(timeMachineState, result, action);

          result = timeMachineState.States[timeMachineState.Position];

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
