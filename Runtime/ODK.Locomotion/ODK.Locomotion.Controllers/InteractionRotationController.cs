using ODK.Interaction.Controllers.Interfaces;
using ODK.Locomotion.ODK.Locomotion.Models;
using ODK.Locomotion.Services.Interfaces;
using ODK.Netcode.Prediction;
using Omni.Attributes;
using Omni.Providers;
using UnityEngine;

namespace ODK.Locomotion.Controllers
{
  public partial class InteractionRotationController : PredictedBehaviour<RotationInputModel, RotationStateModel>
  {
    private const float _speed = 65f;

    [SerializeField]
    private ContainerProvider _inputContainer;

    private ISecondaryDeviceInputController _deviceInputController => _inputContainer.GetLocalInstanceOf<ISecondaryDeviceInputController>();

    private Vector3 _eulerAngles;
    
    [Inject]
    private partial void Inject(
      [Private] IRotationService _rotationService 
    );

    public override void OnNetworkSpawn()
    {
      if (!IsClient || !IsOwner)
        return;

      _deviceInputController.ConnectToInterfaceInputEventStack(OnInput);
    }

    private void OnInput(IDeviceInterfaceInput input)
    {
      _eulerAngles = new Vector3(0, input.ThumbstickValue().x, 0);
    }

    protected override bool ReadInput(out RotationInputModel input)
    {
      input = new RotationInputModel { Rotation = _eulerAngles };
      return true;
    }

    protected override bool Simulate(RotationInputModel input, out RotationStateModel state)
    {
      Vector3 newEulerAngles = _rotationService?.UpdateRotation(
        gameObject,
        input.Rotation,
        _speed
      ) ?? transform.eulerAngles;

      state = new RotationStateModel { EulerAngles = newEulerAngles };
      return true;
    }

    protected override void Reconcile(RotationStateModel reconcileState)
    {
      transform.eulerAngles = reconcileState.EulerAngles;
    }
  }
}