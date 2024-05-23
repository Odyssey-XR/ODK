#nullable enable

namespace ODK.GameObjects.XR
{
  using ODK.Shared.Transforms;
  using ODK.Shared.XR;
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

    /// <summary>
    /// The <see cref="ILocomotionService"/>
    /// </summary>
    protected ILocomotionService _locomotionService = null!;

    /// <summary>
    /// The <see cref="IXRInputEventer"/>
    /// </summary>
    protected IXRInputEventer? _inputEventer;

    /// <inheritdoc cref="MonoBehaviour"/>
    protected virtual void Awake()
    {
      // TODO: Read from a player configuration file and pass info to some service factory
      // to allow for different types of locomotion services.
      _locomotionService = new DefaultLocomotionService();

      _inputEventer = _inputTracker as IXRInputEventer;
      if (_inputEventer is null)
        Debug.LogError("Input tracker must be able to resolve to an IXRInputEventer interface");

      _inputEventer?.Connect(OnInput);
    }

    /// <summary>
    /// Event listener for XR input actions
    /// </summary>
    /// <param name="input">
    /// A <see cref="IXRInput"/>
    /// </param> 
    protected virtual void OnInput(IXRInput input)
    {
      if (_playerRoot is null || !IsOwner)
        return;
      
      Vector2 direction     = input.ThumbstickValue();
      Vector3 positionDelta = _locomotionService.CalculateGroundPositionDelta(direction, _lookDirection);
      SetPlayerPosition(positionDelta);
    }

    /// <summary>
    /// Sets the player's position 
    /// </summary>
    /// <param name="positionDelta">
    /// The delta in rotation the player will rotate
    /// </param>
    protected virtual void SetPlayerPosition(Vector3 positionDelta)
    {
      _playerRoot!.transform.position += positionDelta * _speed;
    }
  }
}