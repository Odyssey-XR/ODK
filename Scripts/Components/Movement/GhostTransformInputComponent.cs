#nullable enable

namespace OdysseyXR.ODK.Components.Movement 
{
  using Unity.Mathematics;
  using Unity.NetCode;

  [GhostComponent(PrefabType = GhostPrefabType.AllPredicted)]
  public struct GhostTransformInputComponent : IInputComponentData
  {
    [GhostField(Quantization = 100)] public float3 Position;
    [GhostField(Quantization = 100)] public float3 Rotation;
  }
}