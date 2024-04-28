#nullable enable

namespace Plugins.ODK.Behaviours.XR
{
  using OdysseyXR.ODK.Attributes.ECS;
  using OdysseyXR.ODK.Behaviours.ECS;
  using OdysseyXR.ODK.Components.Movement;
  using OdysseyXR.ODK.Components.XR;
  using UnityEngine;

  [AutoBaker(typeof(TrackDeviceTransformComponent))]
  [BakeWithTags(typeof(GhostTransformInputComponent))]
  public class DeviceTransformTracker : AuthoredBehaviour
  {
    [SerializeField]
    [BakeAsField(nameof(TrackDeviceTransformComponent.TrackTransform))]
    public bool TrackTransform = true;

    [SerializeField]
    [BakeAsField(nameof(TrackDeviceTransformComponent.DeviceType))]
    public TrackDeviceTransformComponent.TrackableDeviceType DeviceType;
  }
}