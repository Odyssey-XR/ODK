#nullable enable

namespace OdysseyXR.ODK.Components.Player
{
  using Unity.Entities;

  /// <summary>
  /// Sends a request to the server to begin spawning a player
  /// </summary>
  public struct SpawnPlayerServerRequestComponent : IComponentData
  {
    public Entity SourceConnection;
  }
}