namespace DodgeTheCreeps;

using Chickensoft.AutoInject;
using Chickensoft.Collections;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Chickensoft.SaveFileBuilder;
using Godot;

public interface IPlayer :
  IArea2D, IKillable
{
  IPlayerLogic PlayerLogic { get; }

  void Start(Vector2 pos);
}

[Meta(typeof(IAutoNode))]
public partial class Player :
Area2D,
IPlayer,
IProvide<IPlayerLogic>,
IProvide<PlayerLogic.Settings>
{
  public override void _Notification(int what) => this.Notify(what);

  #region Save

  [Dependency]
  public EntityTable EntityTable => this.DependOn<EntityTable>();
  [Dependency]
  public ISaveChunk<GameData> GameChunk => this.DependOn<ISaveChunk<GameData>>();
  public ISaveChunk<PlayerData> PlayerChunk { get; set; } = default!;
  #endregion Save

  #region Provisions

  IPlayerLogic IProvide<IPlayerLogic>.Value() => PlayerLogic;
  PlayerLogic.Settings IProvide<PlayerLogic.Settings>.Value() => Settings;
  #endregion Provisions

  #region Dependencies

  [Dependency]
  public IGameRepo GameRepo => this.DependOn<IGameRepo>();

  [Dependency]
  public IAppRepo AppRepo => this.DependOn<IAppRepo>();

  #endregion Dependencies

  #region Exports

  /// <summary>Player speed (meters/sec).</summary>
  [Export(PropertyHint.Range, "0, 1000, 1")]
  public float MoveSpeed { get; set; } = 400f;

  #endregion Exports

  #region State

  public IPlayerLogic PlayerLogic { get; set; } = default!;
  public PlayerLogic.Settings Settings { get; set; } = default!;

  public PlayerLogic.IBinding PlayerBinding { get; set; } = default!;

  public Vector2 ScreenSize { get; set; } = Vector2.Zero;

  [Node("%PlayerModel")]
  public IPlayerModel PlayerModel { get; set; } = default!;

  [Node("%CollisionShape2D")]
  public ICollisionShape2D CollisionShape2D { get; set; } = default!;

  #endregion State

  public void Setup()
  {
    Settings = new PlayerLogic.Settings(MoveSpeed);

    PlayerLogic = new PlayerLogic();

    PlayerLogic.Set(AppRepo);
    PlayerLogic.Set(GameRepo);
    PlayerLogic.Set(Settings);
    PlayerLogic.Save(() => new PlayerLogic.Data());
  }

  public void OnReady()
  {
    ScreenSize = GetViewportRect().Size;
    BodyEntered += OnBodyEntered;
    Hide();
  }

  public void OnExitTree()
  {
    BodyEntered -= OnBodyEntered;
    PlayerLogic.Stop();
    PlayerBinding.Dispose();
  }

  public void OnResolved()
  {
    PlayerBinding = PlayerLogic.Bind();

    PlayerBinding
      .Handle((in PlayerLogic.Output.Started output) =>
      {
        Position = output.Position;
        Rotation = 0;
        Show();
        CollisionShape2D.Disabled = false;
      })
      .Handle((in PlayerLogic.Output.Moved output) =>
      {
        Rotation = 0;
        Position = output.Position;
        if (output.Direction.Length() > 0)
        {
          PlayerModel.AnimatedSprite2D.Play();
        }
        else
        {
          PlayerModel.AnimatedSprite2D.Stop();
        }

        if (output.Direction.X != 0)
        {
          PlayerModel.AnimatedSprite2D.Animation = "right";
          PlayerModel.AnimatedSprite2D.FlipV = false;
          PlayerModel.Trail.Rotation = 0;
          PlayerModel.AnimatedSprite2D.FlipH = output.Direction.X < 0;
        }
        else if (output.Direction.Y != 0)
        {
          PlayerModel.AnimatedSprite2D.Animation = "up";
          Rotation = output.Direction.Y > 0 ? Mathf.Pi : 0;
        }
      })
      .Handle((in PlayerLogic.Output.Hit output) =>
      {
        Hide();
        // Must be deferred as we can't change physics properties on a physics callback.
        CollisionShape2D.SetDeferred(Godot.CollisionShape2D.PropertyName.Disabled, true);
      });

    PlayerLogic.Start();

    var data = PlayerLogic.Get<PlayerLogic.Data>();
    data.ScreenSize = ScreenSize;
  }

  public override void _Process(double delta)
  {
    var direction = Vector2.Zero;
    if (Input.IsActionPressed(GameInputs.MoveRight))
    {
      direction.X += 1;
    }
    if (Input.IsActionPressed(GameInputs.MoveLeft))
    {
      direction.X -= 1;
    }
    if (Input.IsActionPressed(GameInputs.MoveDown))
    {
      direction.Y += 1;
    }
    if (Input.IsActionPressed(GameInputs.MoveUp))
    {
      direction.Y -= 1;
    }

    PlayerLogic.Input(new PlayerLogic.Input.Moving(delta, direction));
  }

  public void Start(Vector2 pos) => PlayerLogic.Input(new PlayerLogic.Input.Start(pos));

  public void OnBodyEntered(Node2D body)
  {
    GD.Print("Player killed by ", body.Name);
    Kill();
  }

  #region IKillable

  public void Kill() => PlayerLogic.Input(new PlayerLogic.Input.Killed());

  #endregion IKillable
}
