using ODK.Extensions;
using ODK.Interaction.Controllers.Interfaces;
using ODK.Locomotion.Controllers.Interfaces;
using ODK.Locomotion.ODK.Locomotion.Models;
using ODK.Locomotion.Services.Interfaces;
using ODK.Netcode.Prediction;
using Omni.Attributes;
using Omni.Providers;
using UnityEngine;

namespace ODK.Locomotion.Controllers
{
  public partial class InteractionLocomotionController : PredictedBehaviour<MovementInputModel, PositionStateModel>
  {
    private const float _speed = 3f;
    
    // TODO: Refactor this so that there are no more local bindings (at least not that any controller should know of)
    // TODO: That way we don't have to pass the containers as parameters.
    // TODO: Basically what we want to do is create a wrapper around ContainerProvider that is also somehow inherits from a NetworkBehaviour
    // TODO: and then we only bind variables if we're the owner (except for services which should just be global I guess)
    [SerializeField] 
    private ContainerProvider _inputContainer;

    [SerializeField]
    private ContainerProvider _locomotionContainer;

    private Vector3 _direction;
    private Vector3 _forward;
    private Vector3 _right;

    private IPrimaryDeviceInputController _primaryDeviceInputController => _inputContainer.GetLocalInstanceOf<IPrimaryDeviceInputController>();

    private IPointer _devicePointer => _locomotionContainer.GetLocalInstanceOf<IPointer>();

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

    private void OnInput(IDeviceInterfaceInput input)
    {
      Vector2 direction = input.ThumbstickValue();
      _direction = new Vector3(direction.x, 0, direction.y);
      _forward = _devicePointer.Forward.XZ();
      _right = _devicePointer.Right.XZ();
    }

    protected override bool ReadInput(out MovementInputModel input)
    {
      input = new MovementInputModel()
      {
        Direction = _direction,
        Forward = _forward,
        Right = _right,
      };
      return true;
    }

    protected override bool Simulate(MovementInputModel input, out PositionStateModel state)
    {
      Vector3 newPosition = _locomotionService?.UpdatePosition(
        gameObject,
        input.Direction,
        input.Forward,
        input.Right,
        _speed
      ) ?? transform.position;
      
      state = new PositionStateModel { Position = newPosition };
      return true;
    }

    protected override void Reconcile(PositionStateModel reconcileState)
    {
      transform.position = reconcileState.Position;
    }
  }
}