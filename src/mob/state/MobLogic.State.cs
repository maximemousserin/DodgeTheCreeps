namespace DodgeTheCreeps;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class MobLogic
{
  [Meta, Id("mob_logic_state")]
  public abstract partial record State : StateLogic<State>;
}
