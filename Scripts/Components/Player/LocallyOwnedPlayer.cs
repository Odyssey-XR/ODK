#nullable enable

namespace OdysseyXR.ODK.Components.Player
{
  using Unity.Entities;

  public struct LocallyOwnedPlayer : IComponentData
  {
    public Entity Instance;
  }
}