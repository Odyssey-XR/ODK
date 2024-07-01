using ODK.Interaction.Models;
using UnityEngine.InputSystem;

namespace ODK.Interaction.Services.Interfaces
{
  internal interface IDeviceInputReaderService
  {
    TransformInputModel       ReadTransformInput(InputActionAsset inputAction);
    DeviceInterfaceInputModel ReadDeviceInterfaceInput(InputActionAsset inputAction);
  }
}