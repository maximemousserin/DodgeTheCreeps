namespace DodgeTheCreeps;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;
using Chickensoft.Sync.Primitives;

public interface IInGameUILogic : ILogicBlock<InGameUILogic.State>;

// This state machine is nothing more than glue to the game repository.
// If the UI were more sophisticated, it'd be easy to expand on this.

[Meta]
[LogicBlock(typeof(State))]
public partial class InGameUILogic : LogicBlock<InGameUILogic.State>, IInGameUILogic
{
  private AutoValue<int>.Binding? _scoreBinding;

  public override Transition GetInitialState() => To<State>();

  public override void OnStart()
  {
    var gameRepo = Get<IGameRepo>();
    _scoreBinding = gameRepo.ScoreChanged.Bind()
      .OnValue((score) => Context.Output(new Output.ScoreChanged(score)));
  }

  public override void OnStop() => _scoreBinding?.Dispose();
}
