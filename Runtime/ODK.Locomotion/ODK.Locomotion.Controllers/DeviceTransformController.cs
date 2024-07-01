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

    protected override bool ReadInput(out TransformInputModel input)
    {
      input = new TransformInputModel
      {
        Position = _deviceTransform.DevicePosition,
        Rotation = _deviceTransform.DeviceRotation.eulerAngles,
      };
      return true;
    }

    protected override bool Simulate(TransformInputModel input, out TransformStateModel state)
    {
      SetTargetTransform(input.Position, input.Rotation);

      state = new TransformStateModel
      {
        Position = input.Position,
        Rotation = input.Rotation
      };
      return true;
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