using System;
using ODK.Netcode.Enums;

namespace ODK.Netcode.Attributes
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
  public class NetworkMethodAttribute : Attribute
  {
    public readonly NetworkUsage Usage;

    public NetworkMethodAttribute(NetworkUsage usage)
    {
      Usage = usage;
    }
  }
}