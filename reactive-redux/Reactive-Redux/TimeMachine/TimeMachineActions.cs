namespace Redux.TimeMachine
{
  public class TimeMachineActions
  {
    public class UndoAction
    {
    }

    public class RedoAction
    {
    }

    public class ClearAction
    {
    }
  }

  public class PauseTimeMachineAction
  {

  }

  public class SetTimeMachinePositionAction
  {
    public int Position { get; set; }
  }
}
