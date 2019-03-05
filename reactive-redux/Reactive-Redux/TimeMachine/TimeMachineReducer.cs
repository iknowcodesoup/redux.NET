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

        //var previousIndex = previousState.Position;
        //object previousAction;

        //do
        //{
        //  previousAction = previousState.Actions[previousIndex++];
        //} while (previousState.Position < previousState.States.Count - 1 && redoAction.TypeToFind.GetType() != previousAction.GetType());

        //var foundPrevious = previousAction.GetType() == redoAction.TypeToFind.GetType();

        //if (!foundPrevious)
        //  return previousState;

        return previousState
          .WithPosition(previousState.Position + 1)
          .WithIsPaused(true);
      }

      if (action is TimeMachineActions.UndoAction undoAction)
      {
        if (previousState.Position == 0)
          return previousState;

        //var previousIndex = previousState.Position;
        //object previousAction;

        //do
        //{
        //  previousAction = previousState.Actions[previousIndex--];
        //} while (previousIndex >= 0 && undoAction.TypeToFind.GetType() != previousAction.GetType());

        //var foundPrevious = previousAction.GetType() == undoAction.TypeToFind.GetType();

        //if (!foundPrevious)
        //  return previousState;

        return previousState
          .WithPosition(previousState.Position - 1)
          .WithIsPaused(true);
      }

      //if (action is TimeMachineActions.PauseTimeMachineAction)
      //{
      //  return previousState
      //      .WithIsPaused(true);
      //}

      //if (action is TimeMachineActions.SetTimeMachinePositionAction)
      //{
      //  return previousState
      //      .WithPosition(((TimeMachineActions.SetTimeMachinePositionAction)action).Position)
      //      .WithIsPaused(true);
      //}

      if (previousState.IsPaused)
      {
        return previousState;
      }

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
