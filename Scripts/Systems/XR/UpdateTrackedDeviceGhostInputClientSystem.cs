#nullable enable

namespace Plugins.ODK.Systems.XR
{
  using System.Collections.Generic;
  using OdysseyXR.ODK.Components.Movement;
  using OdysseyXR.ODK.Components.Player;
  using OdysseyXR.ODK.Components.XR;
  using Plugins.ODK.Scripts.Interfaces.Input;
  using Unity.Burst;
  using Unity.Entities;
  using Unity.NetCode;

  [BurstCompile]
  [UpdateInGroup(typeof(GhostInputSystemGroup))]
  [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
  public partial class TrackedTransformInputClientSystem : SystemBase
  {
    private Dictionary<TrackDeviceTransformComponent.TrackableDeviceType, IGetTransformInput> _inputSystems = new();

    protected override void OnCreate()
    {
      RequireForUpdate<GhostTransformInputComponent>();
      RequireForUpdate<LocallyOwnedPlayer>();

      var leftControllerInput = new LeftControllerInput();
      leftControllerInput.Enable();
      _inputSystems[TrackDeviceTransformComponent.TrackableDeviceType.LeftController] = leftControllerInput;

      var rightControllerInput = new RightControllerInput();
      rightControllerInput.Enable();
      _inputSystems[TrackDeviceTransformComponent.TrackableDeviceType.RightController] = rightControllerInput;

      var hmdInput = new HMDInput();
      hmdInput.Enable();
      _inputSystems[TrackDeviceTransformComponent.TrackableDeviceType.HMD] = hmdInput;

    }

    protected override void OnUpdate()
    {
      foreach (var (transformInputComponent, deviceComponent) in SystemAPI
                 .Query<RefRW<GhostTransformInputComponent>, RefRO<TrackDeviceTransformComponent>>())
      {
        if (deviceComponent.ValueRO.TrackTransform)
          TrackInput(_inputSystems[deviceComponent.ValueRO.DeviceType], transformInputComponent);
      }
    }

    protected void TrackInput(
      IGetTransformInput transformInput,
      RefRW<GhostTransformInputComponent> transformInputComponent)
    {
      transformInputComponent.ValueRW.Position = transformInput.GetPosition();
      transformInputComponent.ValueRW.Rotation = transformInput.GetRotation().eulerAngles;
    }
  }
}