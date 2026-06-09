namespace DodgeTheCreeps;

using System;
using Chickensoft.Sync.Primitives;
using Godot;

public interface IGameRepo : IDisposable
{
  /// <summary>Event invoked when the game ends.</summary>
  event Action<GameOverReason>? Ended;

  /// <summary>Score changed.</summary>
  IAutoValue<int> ScoreChanged { get; }

  /// <summary>Mouse captured status.</summary>
  IAutoValue<bool> IsMouseCaptured { get; }

  /// <summary>Pause status.</summary>
  IAutoValue<bool> IsPaused { get; }

  /// <summary>Player's position in global coordinates.</summary>
  IAutoValue<Vector2> PlayerGlobalPosition { get; }

  /// <summary>Inform the game that the game ended.</summary>
  /// <param name="reason">Game over reason.</param>
  void OnGameEnded(GameOverReason reason);

  /// <summary>Pauses the game and releases the mouse.</summary>
  void Pause();

  /// <summary>Resumes the game and recaptures the mouse.</summary>
  void Resume();

  /// <summary>Changes whether the mouse is captured or not.</summary>
  /// <param name="isMouseCaptured">
  ///   Whether or not the mouse is captured.
  /// </param>
  void SetIsMouseCaptured(bool isMouseCaptured);

  /// <summary>Sets the player's global position.</summary>
  /// <param name="playerGlobalPosition">
  ///   Player's global position in world
  ///   coordinates.
  /// </param>
  void SetPlayerGlobalPosition(Vector2 playerGlobalPosition);

  /// <summary>
  /// Increase Score
  /// </summary>
  void IncreaseScore();
}

/// <summary>
///   Game repository — stores pure game logic that's not directly related to the
///   game node's overall view.
/// </summary>
public class GameRepo : IGameRepo
{
  public IAutoValue<bool> IsMouseCaptured => _isMouseCaptured;
  private readonly AutoValue<bool> _isMouseCaptured;
  public IAutoValue<bool> IsPaused => _isPaused;
  private readonly AutoValue<bool> _isPaused;
  public IAutoValue<Vector2> PlayerGlobalPosition => _playerGlobalPosition;
  private readonly AutoValue<int> _scoreChanged;
  public IAutoValue<int> ScoreChanged => _scoreChanged;
  private readonly AutoValue<Vector2> _playerGlobalPosition;
  public event Action<GameOverReason>? Ended;
  private bool _disposedValue;

  public GameRepo()
  {
    _isMouseCaptured = new AutoValue<bool>(false);
    _isPaused = new AutoValue<bool>(false);
    _playerGlobalPosition = new AutoValue<Vector2>(Vector2.Zero);
    _scoreChanged = new AutoValue<int>(0);
  }

  internal GameRepo(
    AutoValue<bool> isMouseCaptured,
    AutoValue<bool> isPaused,
    AutoValue<Vector2> playerGlobalPosition,
    AutoValue<int> scoreChanged
  )
  {
    _isMouseCaptured = isMouseCaptured;
    _isPaused = isPaused;
    _playerGlobalPosition = playerGlobalPosition;
    _scoreChanged = scoreChanged;
  }

  public void SetPlayerGlobalPosition(Vector2 playerGlobalPosition) =>
    _playerGlobalPosition.Value = playerGlobalPosition;

  public void SetIsMouseCaptured(bool isMouseCaptured) =>
    _isMouseCaptured.Value = isMouseCaptured;

  public void OnGameEnded(GameOverReason reason)
  {
    _isMouseCaptured.Value = false;
    Pause();
    Ended?.Invoke(reason);
  }

  public void Pause()
  {
    _isMouseCaptured.Value = false;
    _isPaused.Value = true;
  }

  public void Resume()
  {
    _isMouseCaptured.Value = true;
    _isPaused.Value = false;
  }

  #region Internals

  protected void Dispose(bool disposing)
  {
    if (!_disposedValue)
    {
      if (disposing)
      {
        // Dispose managed objects.
        _isMouseCaptured.Dispose();
        _playerGlobalPosition.Dispose();
      }

      _disposedValue = true;
    }
  }

  public void Dispose()
  {
    Dispose(disposing: true);
    GC.SuppressFinalize(this);
  }

  public void IncreaseScore() => _scoreChanged.Value += 1;

  #endregion Internals
}
