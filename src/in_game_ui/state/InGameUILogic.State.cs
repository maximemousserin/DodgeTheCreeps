namespace DodgeTheCreeps;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class InGameUILogic
{
  [Meta, Id("in_game_ui_logic_state")]
  public partial record State : StateLogic<State>;
}
