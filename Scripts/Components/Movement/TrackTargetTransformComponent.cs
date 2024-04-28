namespace OdysseyXR.ODK.Components.Movement
{
  using Unity.Entities;

  public struct TrackTargetTransformComponent : IComponentData
  {
    public Entity Target;
  }
}