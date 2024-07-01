using ODK.Interaction.Enums;
using ODK.Interaction.Models;
using UnityEngine;

namespace ODK.Interaction.Services.Interfaces
{
  internal interface IDeviceInterfaceInputConsumerService
  {
    bool ConsumePressedInput(ref DeviceInterfaceInputModel inputModel, DeviceInterface button);
    bool ConsumeTouchedInput(ref DeviceInterfaceInputModel inputModel, DeviceInterface button);
    float ConsumeTriggerValueInput(ref DeviceInterfaceInputModel inputModel);
    float ConsumeGripValueInput(ref DeviceInterfaceInputModel inputModel);
    Vector2 ConsumeThumbstickValueInput(ref DeviceInterfaceInputModel inputModel);
  }
}