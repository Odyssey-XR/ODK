#nullable enable

namespace ODK.GameObjects.XR
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using ODK.Shared.XR;
  using ODK.Shared.Player;
  using ODK.Shared.Input;
  using Unity.Netcode;
  using UnityEngine;
  using UnityEngine.InputSystem;

  /// <summary>
  /// The XRDevicePhysicalInputTracker class is a networked behaviour that tracks the physical input of an XR device
  /// </summary>
  public class DevicePhysicalInputEventTracker : NetworkBehaviour, IInputEvent, IInputEventer
  {
    /// <summary>
    /// Stack of all input listeners, called one by one
    /// </summary>
    private List<InputCommand> _listenerStack = new();

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
    public override void OnNetworkSpawn()
    {
      if (!IsHost && !IsClient)
        return;

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
    public override void OnNetworkDespawn()
    {
      if (!IsHost && !IsClient)
        return;

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
      // We only want to read the input on the client side and replicate it using a network variable
      if (IsOwner)
        ReadLocalInput_Client();

      // We only want to emit input events on the server
      if (!IsServer)
        return;

      for(int i = _listenerStack.Count - 1; i >= 0; i --)
        _listenerStack[i].Invoke(this);
    }

    /// <summary>
    /// Reads input from local player
    /// </summary>
    protected virtual void ReadLocalInput_Client()
    {
      _pressedInputs.Value   = 0;
      _touchedInputs.Value   = 0;
      _triggerValue.Value    = 0;
      _gripValue.Value       = 0;
      _thumbstickValue.Value = Vector2.zero;

      SetMaskedInput_Authority(_pressedInputs, XRDeviceInputInterface.PrimaryButton, _primaryButtonPressedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetMaskedInput_Authority(_pressedInputs, XRDeviceInputInterface.SecondaryButton, _secondaryButtonPressedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetMaskedInput_Authority(_pressedInputs, XRDeviceInputInterface.MenuButton, _menuButtonPressedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetMaskedInput_Authority(_pressedInputs, XRDeviceInputInterface.Trigger, _triggerValueAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetMaskedInput_Authority(_pressedInputs, XRDeviceInputInterface.Grip, _gripValueAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetMaskedInput_Authority(_pressedInputs, XRDeviceInputInterface.Thumbstick, _thumbstickPressedAction?.ReadValue<float>() > 0 ? 1 : 0);

      SetMaskedInput_Authority(_touchedInputs, XRDeviceInputInterface.PrimaryButton, _primaryButtonTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetMaskedInput_Authority(_touchedInputs, XRDeviceInputInterface.SecondaryButton, _secondaryButtonTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetMaskedInput_Authority(_touchedInputs, XRDeviceInputInterface.MenuButton, _menuButtonTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetMaskedInput_Authority(_touchedInputs, XRDeviceInputInterface.Trigger, _triggerTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetMaskedInput_Authority(_touchedInputs, XRDeviceInputInterface.Grip, _gripTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetMaskedInput_Authority(_touchedInputs, XRDeviceInputInterface.Thumbstick, _thumbstickTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);

      _triggerValue.Value    = _triggerValueAction?.ReadValue<float>() ?? 0;
      _gripValue.Value       = _gripValueAction?.ReadValue<float>() ?? 0;
      _thumbstickValue.Value = _thumbstickValueAction?.ReadValue<Vector2>() ?? Vector2.zero;
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
    protected void SetMaskedInput_Authority(
      NetworkVariable<XRDeviceInputInterface> mask,
      XRDeviceInputInterface button,
      int input)
    {
      if (!IsOwner && !IsServer)
        return;

      mask.Value |= (XRDeviceInputInterface)((int)button * input);
    }

    /// <summary>
    /// Consumes bit masked input values from the network values and resets them after being read
    /// </summary>
    /// <param name="mask">
    /// The bitmask to set the input on
    /// </param>
    /// <param name="button">
    /// The controller button that was interacted with
    /// </param>
    /// <param name="consume">
    /// Should the input be consume and set to a default value after reading
    /// </param>
    /// <returns>
    /// The network value of the button
    /// </returns>
    protected bool ConsumeMaskedInput_Authority(
      NetworkVariable<XRDeviceInputInterface> mask,
      XRDeviceInputInterface button,
      bool consume = true)
    {
      if (!IsOwner && !IsServer)
        return false;

      bool value = (int)(mask.Value & button) != 0;
      if (consume)
        SetMaskedInput_Authority(mask, button, 0);

      return value;
    }

    /// <summary>
    /// Consumes typed input values from the network values and resets them after being read
    /// </summary>
    /// <param name="input">
    /// The input
    /// </param>
    /// <param name="default">
    /// A default value to set the input to after consuming
    /// </param>
    /// <param name="consume">
    /// Should the input be consume and set to a default value after reading
    /// </param>
    /// <returns>
    /// The network input value
    /// </returns>
    protected T ConsumeValueInput_Authority<T>(NetworkVariable<T> input, T @default, bool consume = true)
    {
      if (!IsOwner && !IsServer)
        return @default;

      T value = input.Value;
      if (consume)
        input.Value = @default;

      return value;
    }

    /// <inheritdoc />
    public bool IsPrimaryButtonPressed_Authority(bool consume = true)
    {
      return ConsumeMaskedInput_Authority(_pressedInputs, XRDeviceInputInterface.PrimaryButton, consume);
    }

    /// <inheritdoc />
    public bool IsSecondaryButtonPressed_Authority(bool consume = true)
    {
      return ConsumeMaskedInput_Authority(_pressedInputs, XRDeviceInputInterface.SecondaryButton, consume);
    }

    /// <inheritdoc />
    public bool IsMenuButtonPressed_Authority(bool consume = true)
    {
      return ConsumeMaskedInput_Authority(_pressedInputs, XRDeviceInputInterface.MenuButton, consume);
    }

    /// <inheritdoc />
    public bool IsThumbstickPressed_Authority(bool consume = true)
    {
      return ConsumeMaskedInput_Authority(_pressedInputs, XRDeviceInputInterface.Thumbstick, consume);
    }

    /// <inheritdoc />
    public bool IsTriggerPressed_Authority(bool consume = true)
    {
      return ConsumeMaskedInput_Authority(_pressedInputs, XRDeviceInputInterface.Trigger, consume);
    }

    /// <inheritdoc />
    public bool IsGripPressed_Authority(bool consume = true)
    {
      return ConsumeMaskedInput_Authority(_pressedInputs, XRDeviceInputInterface.Grip, consume);
    }

    /// <inheritdoc />
    public bool IsPrimaryButtonTouched_Authority(bool consume = true)
    {
      return ConsumeMaskedInput_Authority(_touchedInputs, XRDeviceInputInterface.PrimaryButton, consume);
    }

    /// <inheritdoc />
    public bool IsSecondaryButtonTouched_Authority(bool consume = true)
    {
      return ConsumeMaskedInput_Authority(_touchedInputs, XRDeviceInputInterface.SecondaryButton, consume);
    }

    /// <inheritdoc />
    public bool IsMenuButtonTouched_Authority(bool consume = true)
    {
      return ConsumeMaskedInput_Authority(_touchedInputs, XRDeviceInputInterface.MenuButton, consume);
    }

    /// <inheritdoc />
    public bool IsThumbstickTouched_Authority(bool consume = true)
    {
      return ConsumeMaskedInput_Authority(_touchedInputs, XRDeviceInputInterface.Thumbstick, consume);
    }

    /// <inheritdoc />
    public bool IsTriggerTouched_Authority(bool consume = true)
    {
      return ConsumeMaskedInput_Authority(_touchedInputs, XRDeviceInputInterface.Trigger, consume);
    }

    /// <inheritdoc />
    public bool IsGripTouched_Authority(bool consume = true)
    {
      return ConsumeMaskedInput_Authority(_touchedInputs, XRDeviceInputInterface.Grip, consume);
    }

    /// <inheritdoc />
    public float TriggerValue_Authority(bool consume = true)
    {
      return ConsumeValueInput_Authority(_triggerValue, 0, consume);
    }

    /// <inheritdoc />
    public float GripValue_Authority(bool consume = true)
    {
      return ConsumeValueInput_Authority(_gripValue, 0, consume);
    }

    /// <inheritdoc />
    public Vector2 ThumbstickValue_Authority(bool consume = true)
    {
      return ConsumeValueInput_Authority(_thumbstickValue, Vector2.zero, consume);
    }

    /// <inheritdoc />
    public void Connect_Server(InputCommand listener)
    {
      if (IsServer)
        _listenerStack.Add(listener);
    }

    /// <inheritdoc />
    public void Disconnect_Server(InputCommand listener)
    {
      if (!IsServer)
        return;

      _listenerStack.Remove(listener);
    }
  }
}