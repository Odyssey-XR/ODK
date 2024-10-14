namespace ODK.Roslyn.Netcode.Abstract;

public enum NetworkContext
{
  Server,
  Client,
  Owner,
  Everyone
}

public static class NetworkContextExtensions
{
  public static bool CanTransitionTo(this NetworkContext context, NetworkContext target)
  {
    if (context == NetworkContext.Server && target == NetworkContext.Client)
      return false;
    if (context == NetworkContext.Server && target == NetworkContext.Server)
      return false;
    return true;
  }
}