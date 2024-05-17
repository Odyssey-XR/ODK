#nullable enable

namespace ODK.GameObjects.XR
{
  using ODK.Shared.Transforms;
  using Unity.Netcode;
  using UnityEngine;
  using UnityEngine.InputSystem;

  /// <summary>
  /// Tracks the position and rotation of a physical input device of an networked owner
  /// </summary>
  [DisallowMultipleComponent]
  public class XRDeviceTransformInputTracker : NetworkBehaviour, ITransform
  {
    /// <summary>
    /// Input action for reading the XR devices position
    /// </summary>
    [SerializeField]
    protected InputAction? _positionInputAction;

    /// <summary>
    /// Input action for reading the XR devices rotation
    /// </summary>
    [SerializeField]
    protected InputAction? _rotationInputAction;

    /// <summary>
    /// Networked variable for syncing the position input
    /// </summary>
    protected NetworkVariable<Vector3> _position = new(Vector3.zero);

    /// <summary>
    /// Networked variable for syncing the rotation input
    /// </summary>
    protected NetworkVariable<Quaternion> _rotation = new(Quaternion.identity);

    /// <inheritdoc />
    public Vector3 Position
    {
      get => _position.Value;
      set => SetPosition(value);
    }

    /// <inheritdoc />
    public Quaternion Rotation
    {
      get => _rotation.Value;
      set => SetRotation(value);
    }

    /// <summary>
    /// Sets the network synced position only when called by the server or an owner
    /// </summary>
    /// <param name="value">
    /// The new <see cref="Vector3"/> position value
    /// </param>
    protected void SetPosition(Vector3 value)
    {
      if (!IsServer && !IsOwner)
        return;

      _position.Value = value;
    }

    /// <summary>
    /// Sets the network synced rotation only when called by the server or an owner
    /// </summary>
    /// <param name="value">
    /// The new <see cref="Quaternion"/> rotation value
    /// </param>
    protected void SetRotation(Quaternion value)
    {
      if (!IsServer && !IsOwner)
        return;

      _rotation.Value = value;
    }

    /// <summary>
    /// Unity's on enable function
    /// </summary>
    protected virtual void OnEnable()
    {
      _positionInputAction?.Enable();
      _rotationInputAction?.Enable();
    }

    /// <summary>
    /// Unity's on disable event function
    /// </summary>
    protected virtual void OnDisable()
    {
      _positionInputAction?.Disable();
      _rotationInputAction?.Disable();
    }

    /// <summary>
    /// Unity's update event function
    /// </summary>
    protected virtual void Update()
    {
      SetPosition(_positionInputAction?.ReadValue<Vector3>() ?? Vector3.zero);
      SetRotation(_rotationInputAction?.ReadValue<Quaternion>() ?? Quaternion.identity);
    }
  }
}