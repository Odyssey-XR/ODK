#nullable enable

namespace ODK.GameObjects.XR
{
  using ODK.Shared.Transforms;
  using ODK.Shared.XR;
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

      Vector2 direction = input.ThumbstickValue();
      if (direction.x == 0)
        return;

      Quaternion rotationDelta = _locomotionService.CalculateGroundRotationDelta(direction.x);
      SetPlayerRotation(rotationDelta);
    }

    /// <summary>
    /// Sets the player's rotation
    /// </summary>
    /// <param name="rotationDelta">
    /// The delta in rotation the player will rotate
    /// </param>
    protected virtual void SetPlayerRotation(Quaternion rotationDelta)
    {
      _playerRoot!.transform.rotation *= Quaternion.Euler(rotationDelta.eulerAngles * _speed);
    }
  }
}