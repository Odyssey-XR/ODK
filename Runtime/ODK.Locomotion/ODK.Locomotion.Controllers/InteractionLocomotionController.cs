using ODK.Extensions;
using ODK.Interaction.Controllers.Interfaces;
using ODK.Locomotion.Controllers.Interfaces;
using ODK.Locomotion.ODK.Locomotion.Models;
using ODK.Locomotion.Services.Interfaces;
using ODK.Netcode.Prediction;
using Omni.Attributes;
using UnityEngine;

namespace ODK.Locomotion.Controllers
{
  public partial class InteractionLocomotionController : PredictedBehaviour<MovementInputModel, PositionStateModel>
  {
    private const float _speed = 3f;

    private Vector3 _direction;
    private Vector3 _forward;
    private Vector3 _right;

    [Inject]
    private partial void Inject(
      [Private] ILocomotionService _locomotionService,
      [Private] IPrimaryDeviceInputController _primaryDeviceInputController,
      [Private] IPointer _devicePointer
    );

    public override void OnNetworkSpawn()
    {
      if (!IsClient || !IsOwner)
        return;

      this.ConnectToDeviceInput(() => _primaryDeviceInputController, OnInput);
    }

    private void OnInput(IDeviceInterfaceInput input)
    {
      Vector2 direction = input.ThumbstickValue();
      _direction = new Vector3(direction.x, 0, direction.y);
      _forward   = _devicePointer?.Forward.XZ() ?? Vector3.zero;
      _right     = _devicePointer?.Right.XZ() ?? Vector3.zero;
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