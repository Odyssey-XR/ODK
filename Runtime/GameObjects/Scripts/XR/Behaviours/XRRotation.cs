#nullable enable

namespace ODK.GameObjects.XR
{
  using ODK.Shared.Input;
  using ODK.Shared.Transforms;
  using ODK.Shared.Player;
  using Unity.Netcode;
  using UnityEngine;

  /// <summary>
  /// Rotation movement provider tied to XR input actions
  /// </summary>
  public class XRRotation : NetworkBehaviour
  {
    /// <summary>
    /// The speed of rotation
    /// </summary>
    /// <remarks>
    /// TODO: Remove and replace with API call to get player information
    /// </remarks>
    [SerializeField]
    private float _speed;

    /// <summary>
    /// The root object to move
    /// </summary>
    [SerializeField]
    private GameObject? _playerRoot;

    /// <summary>
    /// The input tracker behaviour
    /// </summary>
    [SerializeField]
    private MonoBehaviour? _inputTracker;

    private InputCommand _onInputAction;

    /// <summary>
    /// The <see cref="ILocomotionService"/>
    /// </summary>
    protected ILocomotionService _locomotionService = null!;

    /// <summary>
    /// The <see cref="IInputEventer"/>
    /// </summary>
    protected IInputEventer? _inputEventer;

    /// <inheritdoc cref="MonoBehaviour"/>
    public override void OnNetworkSpawn()
    {
      if (!IsServer)
        return;

      _locomotionService = new DefaultLocomotionService();

      _inputEventer = _inputTracker as IInputEventer;
      if (_inputEventer is null)
        Debug.LogError("Input tracker must be able to resolve to an IXRInputEventer interface");

      _onInputAction = new InputCommand(OnInput_Server);
      _inputEventer?.Connect_Server(_onInputAction);
    }

    /// <summary>
    /// Event listener for XR input actions
    /// </summary>
    /// <param name="inputEvent">
    /// A <see cref="IInputEvent"/>
    /// </param>
    protected virtual void OnInput_Server(IInputEvent inputEvent)
    {
      if (_playerRoot is null || !IsServer)
        return;

      Vector2 direction = inputEvent.ThumbstickValue_Authority();
      if (direction.x == 0)
        return;

      Quaternion rotationDelta = _locomotionService.CalculateGroundRotationDelta(direction.x, _speed);
      _playerRoot!.transform.rotation *= rotationDelta;
    }
  }
}