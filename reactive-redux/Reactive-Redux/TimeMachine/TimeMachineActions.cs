namespace Redux.TimeMachine
{
  public class TimeMachineActions
  {
    internal class UndoAction
    {
    }

    internal class RedoAction
    {
    }

    internal class ClearAction
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
