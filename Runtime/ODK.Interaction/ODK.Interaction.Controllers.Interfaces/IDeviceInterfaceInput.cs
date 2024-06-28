using UnityEngine;

namespace ODK.Interaction.Controllers.Interfaces
{
  public interface IDeviceInterfaceInput
  {
    bool    PrimaryPressed();
    bool    PrimaryTouched();
    bool    SecondaryPressed();
    bool    SecondaryTouched();
    bool    MenuPressed();
    bool    MenuTouched();
    float   TriggerValue();
    bool    TriggerPressed();
    bool    TriggerTouched();
    float   GripValue();
    bool    GripPressed();
    bool    GripTouched();
    Vector2 ThumbstickValue();
    bool    ThumbstickPressed();
    bool    ThumbstickTouched();
  }
}