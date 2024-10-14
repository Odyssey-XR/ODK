using System;
using ODK.Netcode.Enums;

namespace ODK.Netcode.Attributes
{
  [AttributeUsage(AttributeTargets.Method)]
  public class NetworkContextAttribute : Attribute
  {
    public readonly NetworkContext Context;

    public NetworkContextAttribute(NetworkContext context)
    {
      Context = context;
    }
  }
}