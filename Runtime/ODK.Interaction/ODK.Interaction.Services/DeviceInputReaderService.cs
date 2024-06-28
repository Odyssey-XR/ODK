#nullable enable

using ODK.Interaction.Enums;
using ODK.Interaction.Models;
using ODK.Interaction.Services.Interfaces;
using Unity.Burst;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ODK.Interaction.Services
{
  [BurstCompile]
  public struct DeviceInputReaderService : IDeviceInputReaderService
  {
    public TransformInputModel ReadTransformInput(InputActionAsset inputAction)
    {
      return new TransformInputModel
      {
        DevicePosition  = inputAction.FindAction("DevicePosition")?.ReadValue<Vector3>() ?? Vector3.zero,
        DeviceRotation  = inputAction.FindAction("DeviceRotation")?.ReadValue<Quaternion>() ?? Quaternion.identity,
        PointerPosition = inputAction.FindAction("PointerPosition")?.ReadValue<Vector3>() ?? Vector3.zero,
        PointerRotation = inputAction.FindAction("PointerRotation")?.ReadValue<Quaternion>() ?? Quaternion.identity,
      };
    }

    public DeviceInterfaceInputModel ReadDeviceInterfaceInput(InputActionAsset inputAction)
    {
      DeviceInterface pressedInputs = 0;
      DeviceInterface touchedInputs = 0;
      DeviceInterfaceInputModel.SetMaskedInput(ref pressedInputs, DeviceInterface.PrimaryButton,
        inputAction.FindAction("PrimaryPressed")?.ReadValue<float>() > 0 ? 1 : 0);
      DeviceInterfaceInputModel.SetMaskedInput(ref pressedInputs, DeviceInterface.SecondaryButton,
        inputAction.FindAction("SecondaryPressed")?.ReadValue<float>() > 0 ? 1 : 0);
      DeviceInterfaceInputModel.SetMaskedInput(ref pressedInputs, DeviceInterface.MenuButton,
        inputAction.FindAction("MenuPressed")?.ReadValue<float>() > 0 ? 1 : 0);
      DeviceInterfaceInputModel.SetMaskedInput(ref pressedInputs, DeviceInterface.Trigger,
        inputAction.FindAction("TriggerPressed")?.ReadValue<float>() > 0 ? 1 : 0);
      DeviceInterfaceInputModel.SetMaskedInput(ref pressedInputs, DeviceInterface.Grip,
        inputAction.FindAction("GripPressed")?.ReadValue<float>() > 0 ? 1 : 0);
      DeviceInterfaceInputModel.SetMaskedInput(ref pressedInputs, DeviceInterface.Thumbstick,
        inputAction.FindAction("ThumbstickPressed")?.ReadValue<float>() > 0 ? 1 : 0);

      DeviceInterfaceInputModel.SetMaskedInput(ref touchedInputs, DeviceInterface.PrimaryButton,
        inputAction.FindAction("PrimaryTouched")?.ReadValue<float>() > 0 ? 1 : 0);
      DeviceInterfaceInputModel.SetMaskedInput(ref touchedInputs, DeviceInterface.SecondaryButton,
        inputAction.FindAction("SecondaryTouched")?.ReadValue<float>() > 0 ? 1 : 0);
      DeviceInterfaceInputModel.SetMaskedInput(ref touchedInputs, DeviceInterface.MenuButton,
        inputAction.FindAction("MenuTouched")?.ReadValue<float>() > 0 ? 1 : 0);
      DeviceInterfaceInputModel.SetMaskedInput(ref touchedInputs, DeviceInterface.Trigger,
        inputAction.FindAction("TriggerTouched")?.ReadValue<float>() > 0 ? 1 : 0);
      DeviceInterfaceInputModel.SetMaskedInput(ref touchedInputs, DeviceInterface.Grip,
        inputAction.FindAction("GripTouched")?.ReadValue<float>() > 0 ? 1 : 0);
      DeviceInterfaceInputModel.SetMaskedInput(ref touchedInputs, DeviceInterface.Thumbstick,
        inputAction.FindAction("ThumbstickTouched")?.ReadValue<float>() > 0 ? 1 : 0);

      float   triggerValue    = inputAction.FindAction("TriggerValue")?.ReadValue<float>() ?? 0;
      float   gripValue       = inputAction.FindAction("GripValue")?.ReadValue<float>() ?? 0;
      Vector2 thumbstickValue = inputAction.FindAction("ThumbstickValue")?.ReadValue<Vector2>() ?? Vector2.zero;

      return new DeviceInterfaceInputModel
      {
        PressedInputs   = pressedInputs,
        TouchedInputs   = touchedInputs,
        TriggerValue    = triggerValue,
        GripValue       = gripValue,
        ThumbstickValue = thumbstickValue,
      };
    }
  }
}