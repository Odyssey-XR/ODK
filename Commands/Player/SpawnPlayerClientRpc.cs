#nullable enable

namespace OdysseyXR.ODK.Commands.Player
{
  using Unity.NetCode;

  /// <summary>
  /// Sends a request to the server to begin spawning a player
  /// </summary>
  public struct SpawnPlayerClientRpc : IRpcCommand
  {
    public bool FirstEverSpawn;
  }
}