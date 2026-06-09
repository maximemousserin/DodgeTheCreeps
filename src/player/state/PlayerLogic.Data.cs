namespace DodgeTheCreeps;

using Chickensoft.Introspection;
using Godot;

public partial class PlayerLogic
{
  /// <summary>Data shared between states.</summary>
  [Meta, Id("player_logic_data")]
  public partial record Data
  {
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 ScreenSize { get; set; } = Vector2.Zero;
  }
}
