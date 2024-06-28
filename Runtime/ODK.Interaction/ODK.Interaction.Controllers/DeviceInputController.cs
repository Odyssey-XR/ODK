using System;
using System.Collections.Generic;
using ODK.Interaction.Controllers.Interfaces;
using ODK.Interaction.Enums;
using ODK.Interaction.Models;
using ODK.Interaction.Services.Interfaces;
using Omni.Attributes;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ODK.XR.Interaction.Controllers
{
  public partial class DeviceInputController : NetworkBehaviour,
                                               IPrimaryDeviceInputController,
                                               ISecondaryDeviceInputController
  {
    public Vector3    DevicePosition => _transformInputModel.DevicePosition;
    public Quaternion DeviceRotation => _transformInputModel.DeviceRotation;
    public Vector3    PointerForward => _transformInputModel.GetPointerForward();
    public Vector3    PointerRight   => _transformInputModel.GetPointerRight();
    public Vector3    PointerUp      => _transformInputModel.GetPointerUp();

    private TransformInputModel _transformInputModel;

    private DeviceInterfaceInputModel _deviceInterfaceInputModel;

    private List<Action<IDeviceInterfaceInput>> _listenerStack = new();

    [SerializeField]
    private InputActionAsset _deviceInput;

    [Inject]
    private partial void Inject(
      [Private] IDeviceInputReaderService _deviceInputReaderService,
      [Private] IDeviceInterfaceInputConsumerService _deviceInputConsumerService
    );

    public override void OnNetworkSpawn()
    {
      if (!IsClient || !IsOwner)
        return;

      if (_deviceInputReaderService is null)
        Debug.LogError($"Cannot read device input. Make sure a DeviceInputContainer is attached to the player");

      _deviceInput.Enable();
    }

    public override void OnNetworkDespawn()
    {
      if (!IsClient || !IsOwner)
        return;

      _deviceInput.Disable();
    }

    public void ConnectToInterfaceInputEventStack(Action<IDeviceInterfaceInput> listener)
    {
      if (!IsClient)
        return;

      _listenerStack.Add(listener);
    }

    public void DisconnectFromInterfaceInputEventStack(Action<IDeviceInterfaceInput> listener)
    {
      if (!IsClient)
        return;

      _listenerStack.Remove(listener);
    }

    public bool PrimaryPressed()
    {
      return _deviceInputConsumerService?.ConsumePressedInput(ref _deviceInterfaceInputModel,
        DeviceInterface.PrimaryButton) ?? false;
    }

    public bool PrimaryTouched()
    {
      return _deviceInputConsumerService?.ConsumeTouchedInput(ref _deviceInterfaceInputModel,
        DeviceInterface.PrimaryButton) ?? false;
    }

    public bool SecondaryPressed()
    {
      return _deviceInputConsumerService?.ConsumePressedInput(ref _deviceInterfaceInputModel,
        DeviceInterface.SecondaryButton) ?? false;
    }

    public bool SecondaryTouched()
    {
      return _deviceInputConsumerService?.ConsumeTouchedInput(ref _deviceInterfaceInputModel,
        DeviceInterface.SecondaryButton) ?? false;
    }

    public bool MenuPressed()
    {
      return _deviceInputConsumerService?.ConsumePressedInput(ref _deviceInterfaceInputModel,
        DeviceInterface.MenuButton) ?? false;
    }

    public bool MenuTouched()
    {
      return _deviceInputConsumerService?.ConsumeTouchedInput(ref _deviceInterfaceInputModel,
        DeviceInterface.MenuButton) ?? false;
    }

    public float TriggerValue()
    {
      return _deviceInputConsumerService?.ConsumeTriggerValueInput(ref _deviceInterfaceInputModel) ?? 0;
    }

    public bool TriggerPressed()
    {
      return _deviceInputConsumerService?.ConsumePressedInput(ref _deviceInterfaceInputModel,
        DeviceInterface.Trigger) ?? false;
    }

    public bool TriggerTouched()
    {
      return _deviceInputConsumerService?.ConsumeTouchedInput(ref _deviceInterfaceInputModel,
        DeviceInterface.Trigger) ?? false;
    }

    public float GripValue()
    {
      return _deviceInputConsumerService?.ConsumeGripValueInput(ref _deviceInterfaceInputModel) ?? 0;
    }

    public bool GripPressed()
    {
      return _deviceInputConsumerService?.ConsumePressedInput(ref _deviceInterfaceInputModel, DeviceInterface.Grip) ??
             false;
    }

    public bool GripTouched()
    {
      return _deviceInputConsumerService?.ConsumeTouchedInput(ref _deviceInterfaceInputModel, DeviceInterface.Grip) ??
             false;
    }

    public Vector2 ThumbstickValue()
    {
      return _deviceInputConsumerService?.ConsumeThumbstickValueInput(ref _deviceInterfaceInputModel) ?? Vector2.zero;
    }

    public bool ThumbstickPressed()
    {
      return _deviceInputConsumerService?.ConsumePressedInput(ref _deviceInterfaceInputModel,
        DeviceInterface.Thumbstick) ?? false;
    }

    public bool ThumbstickTouched()
    {
      return _deviceInputConsumerService?.ConsumeTouchedInput(ref _deviceInterfaceInputModel,
        DeviceInterface.Thumbstick) ?? false;
    }

    private void Update()
    {
      if (IsClient && IsOwner)
      {
        _transformInputModel       = _deviceInputReaderService!.ReadTransformInput(_deviceInput);
        _deviceInterfaceInputModel = _deviceInputReaderService!.ReadDeviceInterfaceInput(_deviceInput);
      }

      for (int i = _listenerStack.Count - 1; i >= 0; i--)
        _listenerStack[i].Invoke(this);
    }
  }
}