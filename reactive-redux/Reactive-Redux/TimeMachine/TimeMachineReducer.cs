using System.Collections.Generic;
using System.Collections.Immutable;

namespace Redux.TimeMachine
{
  public class TimeMachineReducer
  {
    public static TimeMachineState<TState> Execute<TState>(TimeMachineState<TState> previousState, TState innerState, object action)
    {
      if (action is TimeMachineActions.RedoAction redoAction)
      {
        if (previousState.Position >= previousState.States.Count - 1)
          return previousState;

        var trimPosition = previousState.Position + 1;

        var nextPosition = previousState.Actions
          .GetRange(trimPosition, (previousState.Actions.Count - 1) - previousState.Position)
          .FindIndex(x => x.GetType() == redoAction.TypeToFind);

        var filteredPosition = nextPosition == -1 ? previousState.States.Count - 1 : nextPosition;

        return previousState
          .WithPosition(filteredPosition)
          .WithIsPaused(true);
      }

      if (action is TimeMachineActions.UndoAction undoAction)
      {
        if (previousState.Position == 0)
          return previousState;

        var trimPosition = previousState.Position - 1;

        var nextPosition = previousState.Actions
          .GetRange(0, trimPosition)
          .FindLastIndex(x => x.GetType() == undoAction.TypeToFind);

        var filteredPosition = nextPosition == -1 ? 0 : nextPosition;

        return previousState
          .WithPosition(filteredPosition)
          .WithIsPaused(true);
      }

      if (action is TimeMachineActions.ClearAction)
      {
        return previousState
          .WithActions(new List<object>() { previousState.Actions[previousState.Actions.Count - 1] }.ToImmutableList())
          .WithStates(new List<TState>() { innerState }.ToImmutableList())
          .WithPosition(0);
      }

      if (!action.GetType().FullName.Contains("TimeMachineActions"))
        previousState = previousState
          .WithIsPaused(false);

      if (previousState.IsPaused)
        return previousState;

      //if (previousState.Position < previousState.States.Count - 1)
      //{
      //  previousState = previousState
      //    .WithStates(previousState.States.Take(previousState.Position).ToImmutableList())
      //    .WithActions(previousState.Actions.Take(previousState.Position).ToImmutableList());
      //}

      return previousState
          .WithStates(previousState.States.Add(innerState))
          .WithActions(previousState.Actions.Add(action))
          .WithPosition(previousState.Position + 1);
    }
  }
}
