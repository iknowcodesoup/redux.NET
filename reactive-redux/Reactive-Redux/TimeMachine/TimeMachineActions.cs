using System;

namespace Redux.TimeMachine
{
  public class TimeMachineActions
  {
    public class UndoAction
    {
      Type TypeToFind { get; set; }
    }

    public class RedoAction
    {
      Type TypeToFind { get; set; }
    }

    public class ClearAction
    {
    }

    public class PauseTimeMachineAction
    {

    }

    public class SetTimeMachinePositionAction
    {
      public int Position { get; set; }
    }
  }
}
