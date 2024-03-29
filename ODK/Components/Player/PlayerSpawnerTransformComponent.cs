#nullable enable

namespace OdysseyXR.ODK.Components
{
  using System;
  using Unity.Entities;
  using Unity.Mathematics;

  /// <summary>
  /// Component data for the location and rotation of a player spawner
  /// </summary>
  [Serializable]
  [InternalBufferCapacity(5)]
  public struct PlayerSpawnerTransformComponent : IBufferElementData 
  {
    public float3 Location;
    public float3 Rotation;
  }
}