#nullable enable

namespace OdysseyXR.ODK.Components.XR
{
  using Unity.Entities;

  public struct TrackDeviceTransformComponent : IComponentData
  {
    public enum TrackableDeviceType
    {
      LeftController,
      RightController,
      HMD
    }

    public bool                TrackTransform;
    public TrackableDeviceType DeviceType;
  }
}