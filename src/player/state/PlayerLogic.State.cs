namespace DodgeTheCreeps;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class PlayerLogic
{
  [Meta, Id("player_logic_state")]
  public abstract partial record State : StateLogic<State>;
}
