namespace DodgeTheCreeps;

using Godot;

public partial class PlayerLogic
{
  public static class Input
  {
    public readonly record struct Start(Vector2 Position);
    public readonly record struct Moving(double Delta, Vector2 Direction);
    public readonly record struct Killed;
  }
}
