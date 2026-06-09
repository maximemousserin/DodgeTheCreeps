namespace DodgeTheCreeps;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class GameLogic
{
  [Meta, Id("game_logic_state")]
  public abstract partial record State : StateLogic<State>;
}
