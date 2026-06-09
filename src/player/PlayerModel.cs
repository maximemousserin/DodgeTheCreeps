namespace DodgeTheCreeps;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;

public interface IPlayerModel : INode2D
{
  IAnimatedSprite2D AnimatedSprite2D { get; set; }
  IGpuParticles2D Trail { get; set; }
}

[Meta(typeof(IAutoNode))]
public partial class PlayerModel : Node2D, IPlayerModel
{
  public override void _Notification(int what) => this.Notify(what);

  #region Nodes

  [Node("%AnimatedSprite2D")]
  public IAnimatedSprite2D AnimatedSprite2D { get; set; } = default!;

  [Node("%Trail")]
  public IGpuParticles2D Trail { get; set; } = default!;

  #endregion Nodes
}
