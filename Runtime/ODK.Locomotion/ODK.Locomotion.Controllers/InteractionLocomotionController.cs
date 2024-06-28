using ODK.Extensions;
using ODK.Interaction.Controllers.Interfaces;
using ODK.Locomotion.Services.Interfaces;
using Omni.Attributes;
using Omni.Providers;
using Unity.Netcode;
using UnityEngine;

namespace ODK.Locomotion.Controllers
{
  public partial class InteractionLocomotionController : NetworkBehaviour
  {
    [SerializeField]
    private ContainerProvider _inputContainer;

    private NetworkVariable<Vector3> _direction = new(writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<Vector3> _forward = new(writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<Vector3> _right = new(writePerm: NetworkVariableWritePermission.Owner);

    private IPrimaryDeviceInputController _primaryDeviceInputController =>
      _inputContainer.GetLocalInstanceOf<IPrimaryDeviceInputController>();

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

      float speed = 0.05f;
      Vector3? newPosition = _locomotionService?.UpdatePosition(
        transform,
        _direction.Value,
        _forward.Value,
        _right.Value,
        speed
      );
      transform.position = newPosition ?? transform.position;
    }

    private void OnInput(IDeviceInterfaceInput input)
    {
      Vector2 direction = input.ThumbstickValue();
      _direction.Value  = new Vector3(direction.x, 0, direction.y);
      _forward.Value    = _devicePointer.PointerForward.XZ();
      _right.Value      = _devicePointer.PointerRight.XZ();
    }

    [ServerRpc]
    private void UpdatePosition_ServerRpc(Vector3 direction, Vector3 forward, Vector3 right)
    {
      
    }
  }
}