#nullable enable

namespace ODK.GameObjects.XR
{
  using System;
  using System.Collections.Generic;
  using ODK.Shared.XR;
  using Unity.Netcode;
  using UnityEngine;
  using UnityEngine.InputSystem;

  /// <summary>
  /// The XRDevicePhysicalInputTracker class is a networked behaviour that tracks the physical input of an XR device
  /// </summary>
  public class XRDevicePhysicalInputTracker : NetworkBehaviour, IXRInput, IXRInputEventer
  {
    /// <summary>
    /// Stack of all input listeners, called one by one
    /// </summary>
    private Stack<Action<IXRInput>> _listenerStack = new();

    /// <summary>
    /// Input action for reading if the primary button is pressed
    /// </summary>
    [SerializeField]
    protected InputAction? _primaryButtonPressedAction;

    /// <summary>
    /// Input action for reading if the primary button is touched
    /// </summary>
    [SerializeField]
    protected InputAction? _primaryButtonTouchedAction;

    /// <summary>
    /// Input action for reading if the secondary button is pressed
    /// </summary>
    [SerializeField]
    protected InputAction? _secondaryButtonPressedAction;

    /// <summary>
    /// Input action for reading if the secondary button is touched
    /// </summary>
    [SerializeField]
    protected InputAction? _secondaryButtonTouchedAction;

    /// <summary>
    /// Input action for reading if the menu button is pressed
    /// </summary>
    [SerializeField]
    protected InputAction? _menuButtonPressedAction;

    /// <summary>
    /// Input action for reading if the menu button is touched
    /// </summary>
    [SerializeField]
    protected InputAction? _menuButtonTouchedAction;

    /// <summary>
    /// Input action for reading the trigger value
    /// </summary>
    [SerializeField]
    protected InputAction? _triggerValueAction;

    /// <summary>
    /// Input action for reading if the trigger is touched
    /// </summary>
    [SerializeField]
    protected InputAction? _triggerTouchedAction;

    /// <summary>
    /// Input action for reading the grip value
    /// </summary>
    [SerializeField]
    protected InputAction? _gripValueAction;

    /// <summary>
    /// Input action for reading if the grip is touched
    /// </summary>
    [SerializeField]
    protected InputAction? _gripTouchedAction;

    /// <summary>
    /// Input action for reading if the thumbstick is pressed
    /// </summary>
    [SerializeField]
    protected InputAction? _thumbstickPressedAction;

    /// <summary>
    /// Input action for reading the thumbstick value
    /// </summary>
    [SerializeField]
    protected InputAction? _thumbstickValueAction;

    /// <summary>
    /// Input action for reading if the thumbstick is touched
    /// </summary>
    [SerializeField]
    protected InputAction? _thumbstickTouchedAction;

    /// <summary>
    /// Networked bitmask variable for syncing the pressed inputs
    /// </summary>
    protected NetworkVariable<XRDeviceInputInterface> _pressedInputs { get; } =
      new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    /// <summary>
    /// Networked bitmask variable for syncing the touched inputs
    /// </summary>
    protected NetworkVariable<XRDeviceInputInterface> _touchedInputs { get; } =
      new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    /// <summary>
    /// Networked variable for syncing the trigger value
    /// </summary>
    protected NetworkVariable<float> _triggerValue { get; } =
      new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    /// <summary>
    /// Networked variable for syncing the grip value
    /// </summary>
    protected NetworkVariable<float> _gripValue { get; } =
      new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    /// <summary>
    /// Networked variable for syncing the thumbstick value
    /// </summary>
    protected NetworkVariable<Vector2> _thumbstickValue { get; } =
      new(Vector2.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    /// <inheritdoc cref="MonoBehaviour" />
    protected virtual void OnEnable()
    {
      _primaryButtonPressedAction?.Enable();
      _primaryButtonTouchedAction?.Enable();
      _secondaryButtonPressedAction?.Enable();
      _secondaryButtonTouchedAction?.Enable();
      _menuButtonPressedAction?.Enable();
      _menuButtonTouchedAction?.Enable();
      _triggerValueAction?.Enable();
      _triggerTouchedAction?.Enable();
      _gripValueAction?.Enable();
      _gripTouchedAction?.Enable();
      _thumbstickValueAction?.Enable();
      _thumbstickTouchedAction?.Enable();
    }

    /// <inheritdoc cref="MonoBehaviour" />
    protected virtual void OnDisable()
    {
      _primaryButtonPressedAction?.Disable();
      _primaryButtonTouchedAction?.Disable();
      _secondaryButtonPressedAction?.Disable();
      _secondaryButtonTouchedAction?.Disable();
      _menuButtonPressedAction?.Disable();
      _menuButtonTouchedAction?.Disable();
      _triggerValueAction?.Disable();
      _triggerTouchedAction?.Disable();
      _gripValueAction?.Disable();
      _gripTouchedAction?.Disable();
      _thumbstickValueAction?.Disable();
      _thumbstickTouchedAction?.Disable();
    }

    /// <inheritdoc cref="MonoBehaviour" />
    protected virtual void Update()
    {
      if (!IsOwner)
        return;

      _pressedInputs.Value   = 0;
      _touchedInputs.Value   = 0;
      _triggerValue.Value    = 0;
      _gripValue.Value       = 0;
      _thumbstickValue.Value = Vector2.zero;

      SetInput(_pressedInputs, XRDeviceInputInterface.PrimaryButton, _primaryButtonPressedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_pressedInputs, XRDeviceInputInterface.SecondaryButton, _secondaryButtonPressedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_pressedInputs, XRDeviceInputInterface.MenuButton, _menuButtonPressedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_pressedInputs, XRDeviceInputInterface.Trigger, _triggerValueAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_pressedInputs, XRDeviceInputInterface.Grip, _gripValueAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_pressedInputs, XRDeviceInputInterface.Thumbstick, _thumbstickPressedAction?.ReadValue<float>() > 0 ? 1 : 0);

      SetInput(_touchedInputs, XRDeviceInputInterface.PrimaryButton, _primaryButtonTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_touchedInputs, XRDeviceInputInterface.SecondaryButton, _secondaryButtonTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_touchedInputs, XRDeviceInputInterface.MenuButton, _menuButtonTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_touchedInputs, XRDeviceInputInterface.Trigger, _triggerTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_touchedInputs, XRDeviceInputInterface.Grip, _gripTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_touchedInputs, XRDeviceInputInterface.Thumbstick, _thumbstickTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);

      _triggerValue.Value    = _triggerValueAction?.ReadValue<float>() ?? 0;
      _gripValue.Value       = _gripValueAction?.ReadValue<float>() ?? 0;
      _thumbstickValue.Value = _thumbstickValueAction?.ReadValue<Vector2>() ?? Vector2.zero;

      foreach (Action<IXRInput> listener in _listenerStack)
        listener.Invoke(this);
    }

    /// <summary>
    /// Sets a bitmask input value based on the controller interface that was interacted with
    /// </summary>
    /// <param name="mask">
    /// The bitmask to set the input on
    /// </param>
    /// <param name="button">
    /// The controller button that was interacted with
    /// </param>
    /// <param name="input">
    /// Int flag to enabled or disable the input
    /// </param>
    protected void SetInput(NetworkVariable<XRDeviceInputInterface> mask, XRDeviceInputInterface button, int input)
    {
      mask.Value |= (XRDeviceInputInterface)((int)button * input);
    }

    /// <summary>
    /// Reads input values from the network values and resets them after being read
    /// </summary>
    /// <param name="mask">
    /// The bitmask to set the input on
    /// </param>
    /// <param name="button">
    /// The controller button that was interacted with
    /// </param>
    /// <returns>
    /// The network value of the button
    /// </returns>
    protected bool GetInput(NetworkVariable<XRDeviceInputInterface> mask, XRDeviceInputInterface button)
    {
      bool value = (int)(mask.Value & button) != 0;
      SetInput(mask, button, 0);
      return value;
    }

    /// <inheritdoc />
    public bool IsPrimaryButtonPressed()
    {
      return GetInput(_pressedInputs, XRDeviceInputInterface.PrimaryButton);
    }

    /// <inheritdoc />
    public bool IsSecondaryButtonPressed()
    {
      return GetInput(_pressedInputs, XRDeviceInputInterface.SecondaryButton);
    }

    /// <inheritdoc />
    public bool IsMenuButtonPressed()
    {
      return GetInput(_pressedInputs, XRDeviceInputInterface.MenuButton);
    }

    /// <inheritdoc />
    public bool IsThumbstickPressed()
    {
      return GetInput(_pressedInputs, XRDeviceInputInterface.Thumbstick);
    }

    /// <inheritdoc />
    public bool IsTriggerPressed()
    {
      return GetInput(_pressedInputs, XRDeviceInputInterface.Trigger);
    }

    /// <inheritdoc />
    public bool IsGripPressed()
    {
      return GetInput(_pressedInputs, XRDeviceInputInterface.Grip);
    }

    /// <inheritdoc />
    public bool IsPrimaryButtonTouched()
    {
      return GetInput(_touchedInputs, XRDeviceInputInterface.PrimaryButton);
    }

    /// <inheritdoc />
    public bool IsSecondaryButtonTouched()
    {
      return GetInput(_touchedInputs, XRDeviceInputInterface.SecondaryButton);
    }

    /// <inheritdoc />
    public bool IsMenuButtonTouched()
    {
      return GetInput(_touchedInputs, XRDeviceInputInterface.MenuButton);
    }

    /// <inheritdoc />
    public bool IsThumbstickTouched()
    {
      return GetInput(_touchedInputs, XRDeviceInputInterface.Thumbstick);
    }

    /// <inheritdoc />
    public bool IsTriggerTouched()
    {
      return GetInput(_touchedInputs, XRDeviceInputInterface.Trigger);
    }

    /// <inheritdoc />
    public bool IsGripTouched()
    {
      return GetInput(_touchedInputs, XRDeviceInputInterface.Grip);
    }

    /// <inheritdoc />
    public float TriggerValue()
    {
      float value = _triggerValue.Value;
      _triggerValue.Value = 0;
      return value;
    }

    /// <inheritdoc />
    public float GripValue()
    {
      float value = _gripValue.Value;
      _gripValue.Value = 0;
      return value;
    }

    /// <inheritdoc />
    public Vector2 ThumbstickValue()
    {
      Vector2 value = _thumbstickValue.Value;
      _thumbstickValue.Value = Vector2.zero;
      return value;
    }

    /// <inheritdoc />
    public void Connect(Action<IXRInput> listener)
    {
      if (IsOwner)
        _listenerStack.Push(listener);
    }

    /// <inheritdoc />
    public void Disconnect(Action<IXRInput> listener)
    {
      if (!IsOwner)
        return;
      
      Stack<Action<IXRInput>> newStack = new();
      foreach (Action<IXRInput> action in _listenerStack)
      {
        if (action == listener)
          continue;

        newStack.Push(action);
      }

      _listenerStack.Clear();
      _listenerStack = newStack;
    }
  }
}