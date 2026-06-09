namespace DodgeTheCreeps;

using Chickensoft.Introspection;
public partial class PlayerLogic
{
  public abstract partial record State
  {
    [Meta, Id("player_logic_state_disabled")]
    public partial record Disabled : State, IGet<Input.Start>
    {
      public Transition On(in Input.Start input)
      {
        Get<Data>().Position = input.Position;
        return To<Alive>();
      }
    }
  }
}
