#nullable enable

namespace Plugins.ODK.Components.XR
{
  using Unity.Mathematics;
  using Unity.NetCode;
  using UnityEngine;

  [GhostComponent(PrefabType = GhostPrefabType.AllPredicted)]
  public struct GhostDeviceTransformInputComponent : IInputComponentData
  {
    [GhostField] public float3 LeftControllerPosition;
    [GhostField] public Quaternion LeftControllerRotation;
    
    [GhostField] public float3 RightControllerPosition;
    [GhostField] public Quaternion RightControllerRotation;
    
    [GhostField] public float3 HMDPosition;
    [GhostField] public Quaternion HMDRotation;
  }
}