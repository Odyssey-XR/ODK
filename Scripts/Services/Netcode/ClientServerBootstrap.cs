#nullable enable

namespace OdysseyXR.ODK.Services.NetCode
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
    public override bool Initialize(string defaultWorldName)
    {
      CreateLocalWorld(defaultWorldName);
      return true;
    }
  }
}