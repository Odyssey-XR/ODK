#nullable enable

namespace OdysseyXR.ODK.Components
{
  using Unity.Entities;

  /// <summary>
  /// Component data for a player prefab
  /// </summary>
  public struct PlayerPrefabComponent : IComponentData
  {
    public Entity PlayerPrefab;
  }
}