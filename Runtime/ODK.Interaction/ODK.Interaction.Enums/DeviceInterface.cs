using System;

namespace ODK.Interaction.Enums
{
  [Flags]
  public enum DeviceInterface
  {
    PrimaryButton = 1,
    SecondaryButton = 2,
    MenuButton = 4,
    Trigger = 8,
    Grip = 16,
    Thumbstick = 32
  }
}