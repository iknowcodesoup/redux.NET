using System;

namespace Redux.TimeMachine
{
  public class TimeMachineActions
  {
    public class UndoAction
    {
      public Type TypeToFind { get; set; }
    }

    public class RedoAction
    {
      public Type TypeToFind { get; set; }
    }

    public class ClearAction
    {
    }
  }
}
