#nullable enable

namespace OdysseyXR.ODK.Components
{
  using Unity.Entities;
  using Unity.Mathematics;

  /// <summary>
  /// Component data for the location and rotation of a player spawner
  /// </summary>
  public struct PlayerSpawnerComponent : IBufferElementData
  {
    public float3 Location;
    public float3 Rotation;

    /// <summary>
    /// Flag to determine if this spawner is used for new players joining the game for the first time
    /// </summary>
    public bool   IsStartingSpawn;
  }
}