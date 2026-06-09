namespace DodgeTheCreeps;

public partial class MobLogic
{
  public static class Output
  {
    public readonly record struct AnimationSelected(string AnimationName);
    public readonly record struct Gone;
  }
}
