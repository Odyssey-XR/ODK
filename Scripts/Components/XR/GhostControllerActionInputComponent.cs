#nullable enable

namespace Plugins.ODK.Components.XR
{
  using Unity.Mathematics;
  using Unity.NetCode;

  [GhostComponent(PrefabType = GhostPrefabType.AllPredicted)]
  public struct GhostControllerActionInputComponent : IInputComponentData
  {
    [GhostField] public float2 Thumbstick;
  }
}