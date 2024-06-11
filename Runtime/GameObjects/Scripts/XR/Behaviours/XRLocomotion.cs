#nullable enable

namespace ODK.GameObjects.XR
{
  using ODK.Shared.Input;
  using ODK.Shared.Transforms;
  using ODK.Shared.Player;
  using Unity.Netcode;
  using UnityEngine;

  /// <summary>
  /// Locomotion movement provider tied to XR input actions
  /// </summary> 
  public class XRLocomotion : NetworkBehaviour
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

    /// <summary>
    /// The direction source for the player (i.e The player's forward and right vectors)
    /// </summary>
    [SerializeField]
    private Transform? _lookDirection;

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
      
      // TODO: Read from a player configuration file and pass info to some service factory
      // to allow for different types of locomotion services.
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
      
      Vector2 direction     = inputEvent.ThumbstickValue_Authority();
      Vector3 positionDelta = _locomotionService.CalculateGroundPositionDelta(direction, _lookDirection, _speed);
      _playerRoot!.transform.position += positionDelta;
    }
  }
}