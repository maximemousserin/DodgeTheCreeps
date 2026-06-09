namespace DodgeTheCreeps;

using Chickensoft.GodotNodeInterfaces;
public partial class GameLogic
{
  public static class Input
  {
    public readonly record struct Initialize();

    public readonly record struct EndGame(GameOverReason Reason);

    public readonly record struct PauseButtonPressed;

    public readonly record struct PauseMenuTransitioned;

    public readonly record struct DeathMenuTransitioned;

    public readonly record struct SaveRequested;

    public readonly record struct SaveCompleted;

    public readonly record struct GoToMainMenu;

    public readonly record struct Start;

    public readonly record struct MouseCaptured(bool IsMouseCaptured);

    public readonly record struct MobTimerTimeout;

    public readonly record struct ScoreTimerTimeout;

    public readonly record struct StartTimerTimeout(ITimer MobTimer, ITimer ScoreTimer);
  }
}
