namespace DodgeTheCreeps;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class AppLogic
{
  [Meta, Id("app_logic_state")]
  public abstract partial record State : StateLogic<State>;
}
