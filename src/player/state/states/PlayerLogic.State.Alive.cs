namespace DodgeTheCreeps;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;
using Godot;

public partial class PlayerLogic
{
  public abstract partial record State
  {
    [Meta, Id("player_logic_state_alive")]
    public partial record Alive : State, IGet<Input.Moving>, IGet<Input.Killed>
    {
      public Alive()
      {
        this.OnEnter(() => Output(new Output.Started(Get<Data>().Position)));
      }

      public Transition On(in Input.Moving input)
      {
        var data = Get<Data>();
        var settings = Get<Settings>();

        if (input.Direction.Length() > 0)
        {
          // Normalize to prevent fast diagonal movement
          var velocity = input.Direction.Normalized() * settings.Speed;
          // Ensure movement will remain consistent even if the frame rate changes (delta)
          data.Position += velocity * (float)input.Delta;
          // Prevent Player from leaving the screen
          data.Position = data.Position.Clamp(Vector2.Zero, data.ScreenSize);
        }

        Output(new Output.Moved(data.Position, input.Direction));

        return ToSelf();
      }

      public Transition On(in Input.Killed input)
      {
        Get<IGameRepo>().OnGameEnded(GameOverReason.Lost);
        return To<Disabled>();
      }
    }
  }
}
