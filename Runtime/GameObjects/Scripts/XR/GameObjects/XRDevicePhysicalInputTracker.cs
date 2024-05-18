#nullable enable

namespace ODK.GameObjects.XR
{
  using ODK.Shared.Enums;
  using Unity.Netcode;
  using UnityEngine;
  using UnityEngine.InputSystem;

  /// <summary>
  /// The XRDevicePhysicalInputTracker class is a networked behaviour that tracks the physical input of an XR device
  /// </summary>
  public class XRDevicePhysicalInputTracker : NetworkBehaviour
  {
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
    public NetworkVariable<XRDeviceInputInterface> _pressedInputs { get; } = new();

    /// <summary>
    /// Networked bitmask variable for syncing the touched inputs
    /// </summary>
    public NetworkVariable<XRDeviceInputInterface> _touchedInputs { get; } = new();

    /// <summary>
    /// Networked variable for syncing the trigger value
    /// </summary>
    public NetworkVariable<float> _triggerValue { get; } = new();

    /// <summary>
    /// Networked variable for syncing the grip value
    /// </summary>
    public NetworkVariable<float> _gripValue { get; } = new();

    /// <summary>
    /// Networked variable for syncing the thumbstick value
    /// </summary>
    public NetworkVariable<Vector2> _thumbstickValue { get; } = new();

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
      if (!IsServer && !IsOwner)
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
  }
}