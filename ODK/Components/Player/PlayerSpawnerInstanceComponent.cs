#nullable enable

namespace OdysseyXR.ODK.Components
{
  using Unity.Entities;
  using OdysseyXR.ODK.Behaviours;

  /// <summary>
  /// Component data keeping a reference to the baked <see cref="PlayerSpawnerManagerAuthoring"/> entity
  /// </summary>
  public struct PlayerSpawnerInstanceComponent : IComponentData
  {
    public Entity Instance;
  }
}