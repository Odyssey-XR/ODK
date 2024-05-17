#nullable enable

namespace ODK.GameObjects.XR
{
  using ODK.Shared.Enums;
  using Unity.Netcode;
  using UnityEngine;
  using UnityEngine.InputSystem;

  public class XRDevicePhysicalInputTracker : NetworkBehaviour
  {
    [SerializeField]
    protected InputAction? _primaryButtonPressedAction;

    [SerializeField]
    protected InputAction? _primaryButtonTouchedAction;

    [SerializeField]
    protected InputAction? _secondaryButtonPressedAction;

    [SerializeField]
    protected InputAction? _secondaryButtonTouchedAction;

    [SerializeField]
    protected InputAction? _menuButtonPressedAction;

    [SerializeField]
    protected InputAction? _menuButtonTouchedAction;

    [SerializeField]
    protected InputAction? _triggerValueAction;

    [SerializeField]
    protected InputAction? _triggerTouchedAction;

    [SerializeField]
    protected InputAction? _gripValueAction;

    [SerializeField]
    protected InputAction? _gripTouchedAction;

    [SerializeField]
    protected InputAction? _thumbstickPressedAction;

    [SerializeField]
    protected InputAction? _thumbstickValueAction;

    [SerializeField]
    protected InputAction? _thumbstickTouchedAction;

    public NetworkVariable<XRDeviceInputInterface> _pressedInputs   { get; } = new();
    public NetworkVariable<XRDeviceInputInterface> _touchedInputs   { get; } = new();
    public NetworkVariable<float>                  _triggerValue    { get; } = new();
    public NetworkVariable<float>                  _gripValue       { get; } = new();
    public NetworkVariable<Vector2>                _thumbstickValue { get; } = new();

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

    protected virtual void Update()
    {
      if (!IsServer && !IsOwner)
        return;

      _pressedInputs.Value   = 0;
      _touchedInputs.Value   = 0;
      _triggerValue.Value    = 0;
      _gripValue.Value       = 0;
      _thumbstickValue.Value = Vector2.zero;

      SetInput(_pressedInputs, XRDeviceInputInterface.PrimaryButton,
        _primaryButtonPressedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_pressedInputs, XRDeviceInputInterface.SecondaryButton,
        _secondaryButtonPressedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_pressedInputs, XRDeviceInputInterface.MenuButton,
        _menuButtonPressedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_pressedInputs, XRDeviceInputInterface.Trigger, _triggerValueAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_pressedInputs, XRDeviceInputInterface.Grip, _gripValueAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_pressedInputs, XRDeviceInputInterface.Thumbstick,
        _thumbstickPressedAction?.ReadValue<float>() > 0 ? 1 : 0);

      SetInput(_touchedInputs, XRDeviceInputInterface.PrimaryButton,
        _primaryButtonTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_touchedInputs, XRDeviceInputInterface.SecondaryButton,
        _secondaryButtonTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_touchedInputs, XRDeviceInputInterface.MenuButton,
        _menuButtonTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_touchedInputs, XRDeviceInputInterface.Trigger, _triggerTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_touchedInputs, XRDeviceInputInterface.Grip, _gripTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);
      SetInput(_touchedInputs, XRDeviceInputInterface.Thumbstick,
        _thumbstickTouchedAction?.ReadValue<float>() > 0 ? 1 : 0);

      _triggerValue.Value    = _triggerValueAction?.ReadValue<float>() ?? 0;
      _gripValue.Value       = _gripValueAction?.ReadValue<float>() ?? 0;
      _thumbstickValue.Value = _thumbstickValueAction?.ReadValue<Vector2>() ?? Vector2.zero;

      Debug.Log(_triggerValue.Value);
      Debug.Log(_gripValue.Value);
      Debug.Log(_thumbstickValue.Value);
      Debug.Log("");
    }

    protected void SetInput(NetworkVariable<XRDeviceInputInterface> inputs, XRDeviceInputInterface button, int input)
    {
      inputs.Value |= (XRDeviceInputInterface)((int)button * input);
    }
  }
}