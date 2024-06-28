using ODK.Interaction.Enums;
using UnityEngine;

namespace ODK.Interaction.Models
{
  public struct DeviceInterfaceInputModel
  {
    public DeviceInterface PressedInputs;
    public DeviceInterface TouchedInputs;
    public float TriggerValue;
    public float GripValue;
    public Vector2 ThumbstickValue;

    public static void SetMaskedInput(ref DeviceInterface maskedInput, DeviceInterface button, int value)
    {
      maskedInput |= (DeviceInterface)((int)button * value);
    }

    public static bool MaskedInputContains(DeviceInterface maskedInput, DeviceInterface button)
    {
      return (maskedInput & button) > 0;
    }
  }
}