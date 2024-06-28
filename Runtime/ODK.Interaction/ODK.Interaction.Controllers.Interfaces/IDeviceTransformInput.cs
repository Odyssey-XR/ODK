using UnityEngine;

namespace ODK.Interaction.Controllers.Interfaces
{
  public interface IDeviceTransformInput : IDevicePointerInput
  {
    Vector3    DevicePosition { get; }
    Quaternion DeviceRotation { get; }   
  }
}