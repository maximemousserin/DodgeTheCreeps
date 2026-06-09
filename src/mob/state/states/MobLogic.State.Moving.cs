namespace DodgeTheCreeps;

using System;
using Chickensoft.Introspection;

public partial class MobLogic
{
  public abstract partial record State
  {
    [Meta, Id("mob_logic_state_moving")]
    public partial record Moving : State, IGet<Input.Initialize>, IGet<Input.LeaveScreen>
    {
      public Transition On(in Input.Initialize input)
      {
        if (input.AnimationNames.Length == 0)
        {
          return ToSelf();
        }

        var random = new Random();
        var index = random.Next(input.AnimationNames.Length);
        var selected = input.AnimationNames[index];

        Output(new Output.AnimationSelected(selected));
        return ToSelf();
      }

      public Transition On(in Input.LeaveScreen input)
      {
        Output(new Output.Gone());
        return ToSelf();
      }
    }
  }
}
