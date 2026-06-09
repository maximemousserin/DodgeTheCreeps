namespace DodgeTheCreeps;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class InGameAudioLogic
{
  [Meta, Id("in_game_audio_logic_state")]
  public partial record State : StateLogic<State>
  {
    public State()
    {
      OnAttach(() =>
      {
        var appRepo = Get<IAppRepo>();
        var gameRepo = Get<IGameRepo>();
        gameRepo.Ended += OnGameEnded;
        appRepo.MainMenuEntered += OnMainMenuEntered;
        appRepo.GameEntered += OnGameEntered;
      });

      OnDetach(() =>
      {
        var appRepo = Get<IAppRepo>();
        var gameRepo = Get<IGameRepo>();
        gameRepo.Ended -= OnGameEnded;
        appRepo.MainMenuEntered -= OnMainMenuEntered;
        appRepo.GameEntered -= OnGameEntered;
      });
    }

    public void OnGameEnded(GameOverReason reason)
    {
      Output(new Output.StopGameMusic());

      if (reason is not GameOverReason.Lost)
      {
        return;
      }

      Output(new Output.PlayPlayerDied());
    }

    public void OnMainMenuEntered() =>
      Output(new Output.PlayMainMenuMusic());

    public void OnGameEntered() => Output(new Output.PlayGameMusic());
  }
}
