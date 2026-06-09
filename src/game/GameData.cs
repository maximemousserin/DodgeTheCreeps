namespace DodgeTheCreeps;

using Chickensoft.Introspection;
using Chickensoft.Serialization;

[Meta, Id("game_data")]
public partial record GameData
{
  [Save("player_data")]
  public required PlayerData PlayerData { get; init; }
}
