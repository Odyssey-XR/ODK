using ODK.Extensions;
using ODK.Interaction.Controllers.Interfaces;
using ODK.Locomotion.ODK.Locomotion.Models;
using ODK.Locomotion.Services.Interfaces;
using ODK.Netcode.Prediction;
using Omni.Attributes;
using Omni.Providers;
using UnityEngine;

namespace ODK.Locomotion.Controllers
{
  public partial class InteractionLocomotionController : PredictedBehaviour<MovementInputModel, TransformStateModel>
  {
    private const float _speed = 0.5f;
    
    [SerializeField] 
    private ContainerProvider _inputContainer;

    private Vector3 _direction;
    private Vector3 _forward;
    private Vector3 _right;

    private IPrimaryDeviceInputController _primaryDeviceInputController => _inputContainer.GetLocalInstanceOf<IPrimaryDeviceInputController>();

    private IDevicePointerInput _devicePointer => _inputContainer.GetLocalInstanceOf<IDevicePointerInput>();

    [Inject]
    private partial void Inject(
      [Private] ILocomotionService _locomotionService
    );

    public override void OnNetworkSpawn()
    {
      if (!IsClient || !IsOwner)
        return;

      _primaryDeviceInputController.ConnectToInterfaceInputEventStack(OnInput);
    }

    private void FixedUpdate()
    {
      if (!IsServer)
        return;
    }

    private void OnInput(IDeviceInterfaceInput input)
    {
      Vector2 direction = input.ThumbstickValue();
      _direction = new Vector3(direction.x, 0, direction.y);
      _forward = _devicePointer.PointerForward.XZ();
      _right = _devicePointer.PointerRight.XZ();
    }

    protected override MovementInputModel ReadInput()
    {
      return new MovementInputModel()
      {
        Direction = _direction,
        Forward = _forward,
        Right = _right,
      };
    }

    protected override TransformStateModel Simulate(MovementInputModel input)
    {
      Vector3 newPosition = _locomotionService?.UpdatePosition(
        transform,
        _direction,
        _forward,
        _right,
        _speed
      ) ?? transform.position;
      
      transform.position = newPosition;

      return new TransformStateModel
      {
        Position = newPosition,
      };
    }

    protected override void Reconcile(TransformStateModel reconcileState)
    {
      transform.position = reconcileState.Position;
    }
  }
}