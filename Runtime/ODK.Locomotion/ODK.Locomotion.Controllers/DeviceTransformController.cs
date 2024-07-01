using ODK.Interaction.Controllers.Interfaces;
using ODK.Locomotion.ODK.Locomotion.Models;
using ODK.Netcode.Prediction;
using UnityEngine;

namespace ODK.Locomotion.Controllers
{
  public class DeviceTransformController : PredictedBehaviour<TransformInputModel, TransformStateModel>
  {
    [SerializeField] private MonoBehaviour _deviceTransformInputEventInterface;

    [SerializeField] private Transform _target;

    private IDeviceTransformInput _deviceTransform => (IDeviceTransformInput)_deviceTransformInputEventInterface!;

    public override void OnNetworkSpawn()
    {
      if (_deviceTransformInputEventInterface is not IDeviceTransformInput)
        Debug.LogError($"Provided behaviour does not resolve to an {nameof(IDeviceTransformInput)}");
    }

    protected override TransformInputModel ReadInput()
    {
      return new TransformInputModel
      {
        Position = _deviceTransform.DevicePosition,
        Rotation = _deviceTransform.DeviceRotation.eulerAngles,
      };
    }

    protected override TransformStateModel Simulate(TransformInputModel input)
    {
      SetTargetTransform(input.Position, input.Rotation);

      return new TransformStateModel
      {
        Position = input.Position,
        Rotation = input.Rotation
      };
    }

    protected override void Reconcile(TransformStateModel reconcileState)
    {
      SetTargetTransform(reconcileState.Position, reconcileState.Rotation);
    }

    private void SetTargetTransform(Vector3 position, Vector3 rotation)
    {
      _target.localPosition = position;
      _target.localRotation = Quaternion.Euler(rotation);
    }
  }
}