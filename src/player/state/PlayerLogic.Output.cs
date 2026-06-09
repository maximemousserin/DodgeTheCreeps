namespace DodgeTheCreeps;

using Godot;

public partial class PlayerLogic
{
  public static class Output
  {
    public readonly record struct Started(Vector2 Position);
    public readonly record struct Moved(Vector2 Position, Vector2 Direction);
    public readonly record struct Hit;
  }
}
