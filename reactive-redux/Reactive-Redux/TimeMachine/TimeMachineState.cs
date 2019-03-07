using System;
using System.Collections.Immutable;

namespace Redux.TimeMachine
{
  public class TimeMachineState<TState> : IDisposable
  {
    public ImmutableList<object> Actions { get; private set; }

    public ImmutableList<TState> States { get; private set; }

    public int Position { get; private set; }

    public bool IsPaused { get; private set; }

    public TimeMachineState()
    {
      Actions = ImmutableList<object>.Empty;
      States = ImmutableList<TState>.Empty;
    }

    public TimeMachineState(TState initialState) : this()
    {
      States = States.Add(initialState);
    }

    public TimeMachineState(TimeMachineState<TState> other)
    {
      Actions = other.Actions;
      States = other.States;
      Position = other.Position;
      IsPaused = other.IsPaused;
    }

    public void Clear()
    {
      Actions.Clear();
      States.Clear();
      Position = 0;
      IsPaused = false;
    }

    public TimeMachineState<TState> WithPosition(int position)
    {
      return new TimeMachineState<TState>(this) { Position = position };
    }

    public TimeMachineState<TState> WithIsPaused(bool isPaused)
    {
      return new TimeMachineState<TState>(this) { IsPaused = isPaused };
    }

    public TimeMachineState<TState> WithStates(ImmutableList<TState> states)
    {
      return new TimeMachineState<TState>(this) { States = states };
    }

    public TimeMachineState<TState> WithActions(ImmutableList<object> actions)
    {
      return new TimeMachineState<TState>(this) { Actions = actions };
    }

    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          Clear();
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
