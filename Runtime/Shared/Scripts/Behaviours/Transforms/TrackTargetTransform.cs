#nullable enable

namespace ODK.Shared.Transforms
{
  using Unity.Netcode;
  using UnityEngine;

  /// <summary>
  /// Follows the position and rotation of some target object
  /// </summary>
  [RequireComponent(typeof(OwnerNetworkTransform))]
  public class TrackTargetTransform : NetworkBehaviour
  {
    /// <summary>
    /// The target object
    /// </summary>
    [SerializeField]
    private MonoBehaviour? _targetTransform;

    /// <summary>
    /// The target object resolved to an <see cref="ITransform"/>
    /// </summary>
    public ITransform? TargetTransform => _targetTransform as ITransform;

    /// <summary>
    /// Flag to enable and disable tracking of position
    /// </summary>
    [SerializeField]
    public bool TrackPosition = true;

    /// <summary>
    /// Flag to enable and disable tracking of rotation 
    /// </summary>
    [SerializeField]
    public bool TrackRotation = true;

    /// <inheritdoc cref="MonoBehaviour"/>
    protected virtual void Awake()
    {
      if (TargetTransform is null)
        Debug.LogError("target transform field must resolve to an ITransform interface");
    }

    /// <inheritdoc cref="MonoBehaviour"/>
    protected void Update()
    {
      if (!IsOwner)
        return;

      UpdateTransform(
        TargetTransform?.Position ?? Vector3.zero,
        TargetTransform?.Rotation ?? Quaternion.identity
      );
    }
    
    protected virtual void UpdateTransform(Vector3 targetPosition, Quaternion targetRotation)
    {
      if (TrackPosition)
        transform.localPosition = targetPosition;
      
      if (TrackRotation)
        transform.localRotation = targetRotation;
    }
  }
}