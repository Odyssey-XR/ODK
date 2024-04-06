#nullable enable

namespace OdysseyXR.ODK.Core.Netcode
{
  using Unity.NetCode;

  /// <summary>
  /// Create a custom bootstrap, which enables auto-connect.
  /// The bootstrap can also be used to configure other settings as well as to
  /// manually decide which worlds (client and server) to create based on user input
  /// </summary>
  [UnityEngine.Scripting.Preserve]
  public class AutoBootstrap : ClientServerBootstrap
  {
    /// <summary>
    /// The port the world should connect to
    /// </summary>
    public ushort Port { get; protected set; } = 7979;

    public override bool Initialize(string defaultWorldName)
    {
      AutoConnectPort = Port;
      return base.Initialize(defaultWorldName);
    }
  }
}
