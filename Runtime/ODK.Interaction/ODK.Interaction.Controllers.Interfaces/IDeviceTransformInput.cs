using UnityEngine;

namespace ODK.Interaction.Controllers.Interfaces
{
  public interface IDeviceTransformInput
  {
    Vector3    DevicePosition { get; }
    Quaternion DeviceRotation { get; }   
  }
}