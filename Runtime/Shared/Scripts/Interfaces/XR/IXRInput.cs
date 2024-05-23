namespace ODK.Shared.XR
{
  using UnityEngine;

  /// <summary>
  /// Contract for an XR device's input actions
  /// </summary>
  public interface IXRInput
  {
    /// <summary>
    /// Reads the primary button pressed input from some controller
    /// </summary>
    /// <returns>
    /// True if the primary button was pressed
    /// </returns>
    bool IsPrimaryButtonPressed();

    /// <summary>
    /// Reads the secondary button pressed input from some controller
    /// </summary>
    /// <returns>
    /// True if the seconday button was pressed
    /// </returns>
    bool IsSecondaryButtonPressed();

    /// <summary>
    /// Reads the menu button pressed input from some controller
    /// </summary>
    /// <returns>
    /// True if the menu button was pressed
    /// </returns>
    bool IsMenuButtonPressed();

    /// <summary>
    /// Reads the thumbstick pressed input from some controller
    /// </summary>
    /// <returns>
    /// True if the thumbstick was pressed
    /// </returns>
    bool IsThumbstickPressed();

    /// <summary>
    /// Reads the trigger button pressed input from some controller
    /// </summary>
    /// <returns>
    /// True if the trigger button was pressed
    /// </returns>
    bool IsTriggerPressed();

    /// <summary>
    /// Reads the grip button pressed input from some controller
    /// </summary>
    /// <returns>
    /// True if the grip button was pressed
    /// </returns>
    bool IsGripPressed();

    /// <summary>
    /// Reads the primary button touched input from some controller
    /// </summary>
    /// <returns>
    /// True if the primary button was touched 
    /// </returns>
    bool IsPrimaryButtonTouched();

    /// <summary>
    /// Reads the secondary button touched input from some controller
    /// </summary>
    /// <returns>
    /// True if the secondary button was touched 
    /// </returns>
    bool IsSecondaryButtonTouched();

    /// <summary>
    /// Reads the menu button touched input from some controller
    /// </summary>
    /// <returns>
    /// True if the menu button was touched 
    /// </returns>
    bool IsMenuButtonTouched();

    /// <summary>
    /// Reads the thumbstick touched input from some controller
    /// </summary>
    /// <returns>
    /// True if the thumbstick was touched 
    /// </returns>
    bool IsThumbstickTouched();

    /// <summary>
    /// Reads the trigger button touched input from some controller
    /// </summary>
    /// <returns>
    /// True if the trigger button was touched 
    /// </returns>
    bool IsTriggerTouched();

    /// <summary>
    /// Reads the grip button touched input from some controller
    /// </summary>
    /// <returns>
    /// True if the grip button was touched 
    /// </returns>
    bool IsGripTouched();

    /// <summary>
    /// Reads the trigger value input from some controller
    /// </summary>
    /// <returns>
    /// The value of the trigger button
    /// </returns>
    float TriggerValue();

    /// <summary>
    /// Reads the grip value input from some controller
    /// </summary>
    /// <returns>
    /// The value of the grip button
    /// </returns>
    float GripValue();

    /// <summary>
    /// Reads the thumbstick value input from some controller
    /// </summary>
    /// <returns>
    /// The value of the thumbstick
    /// </returns>
    Vector2 ThumbstickValue();
  }
}