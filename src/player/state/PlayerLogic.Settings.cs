namespace DodgeTheCreeps;

public partial class PlayerLogic
{
  /// <summary>Player settings.</summary>
  /// <param name="Speed">How fast the player will move (pixels/sec).</param>
  public record Settings(
    float Speed
  );
}
