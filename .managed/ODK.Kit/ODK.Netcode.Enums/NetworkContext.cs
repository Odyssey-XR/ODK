namespace ODK.Netcode.Enums;

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
    return context switch
    {
      NetworkContext.Server when target == NetworkContext.Client => false,
      NetworkContext.Client when target == NetworkContext.Server => false,
      _                                                          => true
    };
  }
}