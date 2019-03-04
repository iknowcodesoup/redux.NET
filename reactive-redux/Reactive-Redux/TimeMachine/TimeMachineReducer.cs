using System.Collections.Immutable;
using System.Linq;
using System.Reactive.Linq;

namespace Redux.TimeMachine
{
  public class TimeMachineReducer
  {
    public static TimeMachineState<TState> Execute<TState>(TimeMachineState<TState> previousState, TState innerState, object action)
    {
      if (action is TimeMachineActions.RedoAction)
      {
        if (previousState.Position < previousState.States.Count)
          return previousState
            .WithIsPaused(false)
            .WithStates(previousState.States.Take(previousState.Position + 1).ToImmutableList())
            .WithActions(previousState.Actions.Take(previousState.Position).ToImmutableList());
      }

      if (action is TimeMachineActions.PauseTimeMachineAction)
      {
        return previousState
            .WithIsPaused(true);
      }

      if (action is TimeMachineActions.UndoAction actionTyped)
      {
        if (previousState.Position > 0)
          return previousState.WithPosition(previousState.Position - 1)
            .WithIsPaused(true);
      }

      if (action is TimeMachineActions.SetTimeMachinePositionAction)
      {
        return previousState
            .WithPosition(((TimeMachineActions.SetTimeMachinePositionAction)action).Position)
            .WithIsPaused(true);
      }

      if (previousState.IsPaused)
      {
        return previousState;
      }

      return previousState
          .WithStates(previousState.States.Add(innerState))
          .WithActions(previousState.Actions.Add(action))
          .WithPosition(previousState.Position + 1);
    }
  }
}
