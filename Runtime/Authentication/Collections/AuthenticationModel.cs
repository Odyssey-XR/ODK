using ODK.Authentication.Enums;

namespace ODK.Authentication.Collections
{
  public record AuthenticationModel
  {
    public string PlayerId { get; }
    public AuthenticationState State { get; }

    public AuthenticationModel(string playerId, AuthenticationState state)
    {
      PlayerId = playerId;
      State = state;
    }
  }
}