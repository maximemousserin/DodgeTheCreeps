namespace DodgeTheCreeps;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;
using Godot;

public partial class GameLogic
{
  public partial record State
  {
    [Meta]
    public partial record Playing : State,
    IGet<Input.EndGame>,
    IGet<Input.PauseButtonPressed>,
    IGet<Input.MobTimerTimeout>,
    IGet<Input.ScoreTimerTimeout>,
    IGet<Input.StartTimerTimeout>
    {
      public Playing()
      {
        this.OnEnter(
          () =>
          {
            Output(new Output.StartGame());
            Get<IGameRepo>().SetIsMouseCaptured(true);
          }
        );

        OnAttach(() => Get<IGameRepo>().Ended += OnEnded);
        OnDetach(() => Get<IGameRepo>().Ended -= OnEnded);
      }

      public void OnEnded(GameOverReason reason)
        => Input(new Input.EndGame(reason));

      public Transition On(in Input.EndGame input)
      {
        Get<IGameRepo>().Pause();

        return input.Reason switch
        {
          GameOverReason.Lost => To<Lost>(),
          GameOverReason.Quit => To<Quit>(),
          _ => To<Quit>()
        };
      }

      public Transition On(in Input.PauseButtonPressed input) => To<Paused>();

      public Transition On(in Input.MobTimerTimeout input)
      {
        GD.Print("MobTimerTimeout");
        Output(new Output.SpawnMob());
        return ToSelf();
      }

      public Transition On(in Input.ScoreTimerTimeout input)
      {
        GD.Print("ScoreTimerTimeout");
        Get<IGameRepo>().IncreaseScore();
        return ToSelf();
      }

      public Transition On(in Input.StartTimerTimeout input)
      {
        GD.Print("StartTimerTimeout", input);
        input.MobTimer.Start();
        input.ScoreTimer.Start();
        return ToSelf();
      }
    }
  }
}
