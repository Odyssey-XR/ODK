#nullable enable

namespace OdysseyXR.ODK.Components.NetCode
{
  using Unity.NetCode;

  /// <summary>
  /// Sends a request to the server to join the game and start synchronizing and receiving snapshots
  /// </summary>
  public struct JoinWorldClientRpc : IRpcCommand
  {
  }
}