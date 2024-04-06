#nullable enable

namespace OdysseyXR.ODK.Commands.NetCode
{
  using Unity.NetCode;

  /// <summary>
  /// Sends a request to the server to join the game and start synchronizing and receiving snapshots
  /// </summary>
  public struct JoinGameClientRpc : IRpcCommand
  {
  }
}