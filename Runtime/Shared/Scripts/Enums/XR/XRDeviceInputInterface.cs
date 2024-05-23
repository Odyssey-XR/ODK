namespace ODK.Shared.XR
{
  using System;

  /// <summary>
  /// The input interfaces available on an XR device.
  /// </summary>
  [Flags]
  public enum XRDeviceInputInterface
  {
    PrimaryButton   = 1,
    SecondaryButton = 2,
    MenuButton      = 4,
    Trigger         = 8,
    Grip            = 16,
    Thumbstick      = 32,
  }
}