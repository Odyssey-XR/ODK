#nullable enable

namespace ODK.Shared.Player
{
  public interface IPlayer
  {
    IInputEventer? PrimaryInput   { get; }
    IInputEventer? SecondaryInput { get; }
  }
}