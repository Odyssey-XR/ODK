#nullable enable

namespace Plugins.ODK.Systems.XR
{
  using OdysseyXR.ODK.Components.Player;
  using Plugins.ODK.Components.XR;
  using Unity.Entities;
  using Unity.NetCode;
  using UnityEngine;
  using Logger = OdysseyXR.ODK.Services.Logging.Logger;

  [UpdateInGroup(typeof(GhostInputSystemGroup))]
  [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
  public partial class TrackedDeviceTransformInputClientSystem : SystemBase
  {
    private TrackedDeviceTransformInput? _trackedDeviceTransforms;
    private ControllerActionInput?       _controllerActions;

    protected override void OnCreate()
    {
      RequireForUpdate<GhostDeviceTransformInputComponent>();
      RequireForUpdate<LocallyOwnedPlayerTag>();

      _trackedDeviceTransforms = new TrackedDeviceTransformInput();
      _controllerActions       = new ControllerActionInput();
      
      _trackedDeviceTransforms.Enable();
      _controllerActions.Enable();
    }

    protected override void OnUpdate()
    {
      if (_trackedDeviceTransforms is null || _controllerActions is null)
        return;
      
      var leftControllerPosition  = _trackedDeviceTransforms.LeftController.Position.ReadValue<Vector3>();
      var leftControllerRotation  = _trackedDeviceTransforms.LeftController.Rotation.ReadValue<Quaternion>();
      var rightControllerPosition = _trackedDeviceTransforms.RightController.Position.ReadValue<Vector3>();
      var rightControllerRotation = _trackedDeviceTransforms.RightController.Rotation.ReadValue<Quaternion>();
      var hmdPosition             = _trackedDeviceTransforms.HMD.Position.ReadValue<Vector3>();
      var hmdRotation             = _trackedDeviceTransforms.HMD.Rotation.ReadValue<Quaternion>();

      var leftControllerPrimaryPressed    = _controllerActions.LeftController.PrimaryPressed.ReadValue<float>();
      var leftControllerPrimaryTouched    = _controllerActions.LeftController.PrimaryTouched.ReadValue<float>();
      var leftControllerSecondaryPressed  = _controllerActions.LeftController.SecondaryPressed.ReadValue<float>();
      var leftControllerSecondaryTouched  = _controllerActions.LeftController.SecondaryTouched.ReadValue<float>();
      var leftControllerThumbstickPressed = _controllerActions.LeftController.ThumbstickPressed.ReadValue<float>();
      var leftControllerThumbstickTouched = _controllerActions.LeftController.ThumbstickTouched.ReadValue<float>();
      var leftControllerTriggerPressed    = _controllerActions.LeftController.TriggerPressed.ReadValue<float>();
      var leftControllerTriggerTouched    = _controllerActions.LeftController.TriggerTouched.ReadValue<float>();
      var leftControllerGripPressed       = _controllerActions.LeftController.GripPressed.ReadValue<float>();
      var leftControllerGripTouched       = _controllerActions.LeftController.GripTouched.ReadValue<float>();

      var rightControllerPrimaryPressed    = _controllerActions.RightController.PrimaryPressed.ReadValue<float>();
      var rightControllerPrimaryTouched    = _controllerActions.RightController.PrimaryTouched.ReadValue<float>();
      var rightControllerSecondaryPressed  = _controllerActions.RightController.SecondaryPressed.ReadValue<float>();
      var rightControllerSecondaryTouched  = _controllerActions.RightController.SecondaryTouched.ReadValue<float>();
      var rightControllerThumbstickPressed = _controllerActions.RightController.ThumbstickPressed.ReadValue<float>();
      var rightControllerThumbstickTouched = _controllerActions.RightController.ThumbstickTouched.ReadValue<float>();
      var rightControllerTriggerPressed    = _controllerActions.RightController.TriggerPressed.ReadValue<float>();
      var rightControllerTriggerTouched    = _controllerActions.RightController.TriggerTouched.ReadValue<float>();
      var rightControllerGripPressed       = _controllerActions.RightController.GripPressed.ReadValue<float>();
      var rightControllerGripTouched       = _controllerActions.RightController.GripTouched.ReadValue<float>();
      var rightControllerThumbstick        = _controllerActions.RightController.Thumbstick.ReadValue<Vector2>();
        
      foreach (var (ghostDeviceTransformInput, ghostControllerActionInput) in SystemAPI
                 .Query<RefRW<GhostDeviceTransformInputComponent>, RefRW<GhostControllerActionInputComponent>>()
                 .WithAll<LocallyOwnedPlayerTag>())
      {
        Logger.Log(rightControllerThumbstick.ToString());
        
        ghostDeviceTransformInput.ValueRW.LeftControllerPosition  = leftControllerPosition;
        ghostDeviceTransformInput.ValueRW.LeftControllerRotation  = leftControllerRotation;
        ghostDeviceTransformInput.ValueRW.RightControllerPosition = rightControllerPosition;
        ghostDeviceTransformInput.ValueRW.RightControllerRotation = rightControllerRotation;
        ghostDeviceTransformInput.ValueRW.HMDPosition             = hmdPosition;
        ghostDeviceTransformInput.ValueRW.HMDRotation             = hmdRotation;

        ghostControllerActionInput.ValueRW.Thumbstick = rightControllerThumbstick;
      }
    }
  }
}