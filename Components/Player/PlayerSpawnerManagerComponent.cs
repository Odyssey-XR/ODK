#nullable enable

namespace OdysseyXR.ODK.Components
{
  using Unity.Entities;

  /// <summary>
  /// Component data for the player spawner manager
  /// </summary>
  public struct PlayerSpawnerManagerComponent : IComponentData
  {
    public Entity Instance;
    public Entity PlayerPrefab;
  }
}