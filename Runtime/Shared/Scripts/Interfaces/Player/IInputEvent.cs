namespace ODK.Shared.Player
{
  using UnityEngine;

  /// <summary>
  /// Contract for an XR device's input actions
  /// </summary>
  public interface IInputEvent
  {
    /// <summary>
    /// Reads the primary button pressed input from some controller
    /// </summary>
    /// <returns>
    /// True if the primary button was pressed
    /// </returns>
    bool IsPrimaryButtonPressed_Authority(bool consume=true);

    /// <summary>
    /// Reads the secondary button pressed input from some controller
    /// </summary>
    /// <returns>
    /// True if the secondary button was pressed
    /// </returns>
    bool IsSecondaryButtonPressed_Authority(bool consume=true);

    /// <summary>
    /// Reads the menu button pressed input from some controller
    /// </summary>
    /// <returns>
    /// True if the menu button was pressed
    /// </returns>
    bool IsMenuButtonPressed_Authority(bool consume=true);

    /// <summary>
    /// Reads the thumbstick pressed input from some controller
    /// </summary>
    /// <returns>
    /// True if the thumbstick was pressed
    /// </returns>
    bool IsThumbstickPressed_Authority(bool consume=true);

    /// <summary>
    /// Reads the trigger button pressed input from some controller
    /// </summary>
    /// <returns>
    /// True if the trigger button was pressed
    /// </returns>
    bool IsTriggerPressed_Authority(bool consume=true);

    /// <summary>
    /// Reads the grip button pressed input from some controller
    /// </summary>
    /// <returns>
    /// True if the grip button was pressed
    /// </returns>
    bool IsGripPressed_Authority(bool consume=true);

    /// <summary>
    /// Reads the primary button touched input from some controller
    /// </summary>
    /// <returns>
    /// True if the primary button was touched 
    /// </returns>
    bool IsPrimaryButtonTouched_Authority(bool consume=true);

    /// <summary>
    /// Reads the secondary button touched input from some controller
    /// </summary>
    /// <returns>
    /// True if the secondary button was touched 
    /// </returns>
    bool IsSecondaryButtonTouched_Authority(bool consume=true);

    /// <summary>
    /// Reads the menu button touched input from some controller
    /// </summary>
    /// <returns>
    /// True if the menu button was touched 
    /// </returns>
    bool IsMenuButtonTouched_Authority(bool consume=true);

    /// <summary>
    /// Reads the thumbstick touched input from some controller
    /// </summary>
    /// <returns>
    /// True if the thumbstick was touched 
    /// </returns>
    bool IsThumbstickTouched_Authority(bool consume=true);

    /// <summary>
    /// Reads the trigger button touched input from some controller
    /// </summary>
    /// <returns>
    /// True if the trigger button was touched 
    /// </returns>
    bool IsTriggerTouched_Authority(bool consume=true);

    /// <summary>
    /// Reads the grip button touched input from some controller
    /// </summary>
    /// <returns>
    /// True if the grip button was touched 
    /// </returns>
    bool IsGripTouched_Authority(bool consume=true);

    /// <summary>
    /// Reads the trigger value input from some controller
    /// </summary>
    /// <returns>
    /// The value of the trigger button
    /// </returns>
    float TriggerValue_Authority(bool consume=true);

    /// <summary>
    /// Reads the grip value input from some controller
    /// </summary>
    /// <returns>
    /// The value of the grip button
    /// </returns>
    float GripValue_Authority(bool consume=true);

    /// <summary>
    /// Reads the thumbstick value input from some controller
    /// </summary>
    /// <returns>
    /// The value of the thumbstick
    /// </returns>
    Vector2 ThumbstickValue_Authority(bool consume=true);
  }
}