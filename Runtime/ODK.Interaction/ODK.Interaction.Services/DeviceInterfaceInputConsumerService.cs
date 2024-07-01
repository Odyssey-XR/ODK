using ODK.Interaction.Enums;
using ODK.Interaction.Models;
using ODK.Interaction.Services.Interfaces;
using Unity.Burst;
using UnityEngine;

namespace ODK.Interaction.Services
{
  [BurstCompile]
  internal struct DeviceInterfaceInputConsumerService : IDeviceInterfaceInputConsumerService
  {
    public bool ConsumePressedInput(ref DeviceInterfaceInputModel inputModel, DeviceInterface button)
    {
      bool result = DeviceInterfaceInputModel.MaskedInputContains(inputModel.PressedInputs, button);
      DeviceInterfaceInputModel.SetMaskedInput(ref inputModel.PressedInputs, button, 0);
      return result;
    }

    public bool ConsumeTouchedInput(ref DeviceInterfaceInputModel inputModel, DeviceInterface button)
    {
      bool result = DeviceInterfaceInputModel.MaskedInputContains(inputModel.TouchedInputs, button);
      DeviceInterfaceInputModel.SetMaskedInput(ref inputModel.TouchedInputs, button, 0);
      return result;
    }

    public float ConsumeTriggerValueInput(ref DeviceInterfaceInputModel inputModel)
    {
      float result = inputModel.TriggerValue;
      inputModel.TriggerValue = 0;
      return result;
    }

    public float ConsumeGripValueInput(ref DeviceInterfaceInputModel inputModel)
    {
      float result = inputModel.GripValue;
      inputModel.GripValue = 0;
      return result;
    }

    public Vector2 ConsumeThumbstickValueInput(ref DeviceInterfaceInputModel inputModel)
    {
      Vector2 result = inputModel.ThumbstickValue;
      inputModel.ThumbstickValue = Vector2.zero;
      return result;
    }
  }
}