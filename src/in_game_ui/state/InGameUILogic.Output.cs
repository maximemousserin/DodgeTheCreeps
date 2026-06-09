namespace DodgeTheCreeps;

public partial class InGameUILogic
{
  public static class Output
  {
    public readonly record struct ScoreChanged(
      int Score
    );
  }
}
