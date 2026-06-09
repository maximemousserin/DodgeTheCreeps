namespace DodgeTheCreeps;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;

public interface IMob : IRigidBody2D
{
  IMobLogic MobLogic { get; }
}

[Meta(typeof(IAutoNode))]
public partial class Mob : RigidBody2D, IMob
{
  public override void _Notification(int what) => this.Notify(what);

  public IMobLogic MobLogic { get; set; } = default!;
  public MobLogic.IBinding MobBinding { get; set; } = default!;

  [Node("AnimatedSprite2D")]
  public IAnimatedSprite2D AnimatedSprite2D { get; set; } = default!;

  public void Setup() => MobLogic = new MobLogic();

  public void OnReady()
  {
    MobBinding = MobLogic.Bind();

    MobBinding
        .Handle((in MobLogic.Output.AnimationSelected output) =>
        {
          AnimatedSprite2D.Animation = output.AnimationName;
          AnimatedSprite2D.Play();
        })
        .Handle((in MobLogic.Output.Gone output) => QueueFree());

    MobLogic.Start();

    var names = AnimatedSprite2D.SpriteFrames.GetAnimationNames();
    MobLogic.Input(new MobLogic.Input.Initialize(names));
  }

  public void OnExitTree()
  {
    MobLogic.Stop();
    MobBinding.Dispose();
  }

  public void OnVisibleOnScreenNotifier2DScreenExited() => MobLogic.Input(new MobLogic.Input.LeaveScreen());
}
