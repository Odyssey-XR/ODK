#nullable enable

namespace Plugins.ODK.Behaviours.XR
{
  using Plugins.ODK.Components.XR;
  using Unity.Entities;
  using UnityEngine;

  public class TrackedDeviceTransformInputBehaviour : MonoBehaviour
  {
  }

  public class TrackedInputBaker : Baker<TrackedDeviceTransformInputBehaviour>
  {
    public override void Bake(TrackedDeviceTransformInputBehaviour authoring)
    {
      var entity = GetEntity(TransformUsageFlags.None);
      AddComponent<GhostDeviceTransformInputComponent>(entity);
    }
  }
}