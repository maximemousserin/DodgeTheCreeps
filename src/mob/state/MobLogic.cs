namespace DodgeTheCreeps;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public interface IMobLogic : ILogicBlock<MobLogic.State>;

[Meta, Id("mob_logic")]
[LogicBlock(typeof(State), Diagram = true)]
public partial class MobLogic : LogicBlock<MobLogic.State>, IMobLogic
{
  public override Transition GetInitialState() => To<State.Moving>();
}
