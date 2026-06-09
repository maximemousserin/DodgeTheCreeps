namespace DodgeTheCreeps;

public partial class MobLogic
{
  public static class Input
  {
    public readonly record struct Initialize(string[] AnimationNames);
    public readonly record struct LeaveScreen;
  }
}
