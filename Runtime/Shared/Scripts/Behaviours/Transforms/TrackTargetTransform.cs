#nullable enable

namespace ODK.Shared.Scripts.Behaviours.Transforms
{
  using ODK.Shared.Transforms;
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
    protected ITransform? TargetTransform => _targetTransform as ITransform;

    /// <summary>
    /// The owner's network transform
    /// </summary>
    protected OwnerNetworkTransform SourceNetworkTransform = null!;

    /// <summary>
    /// Unity's Awake event function
    /// </summary>
    protected virtual void Awake()
    {
      if (TargetTransform is null)
        Debug.LogError("target transform field must resolver to an ITransform interface");

      SourceNetworkTransform = GetComponent<OwnerNetworkTransform>();
    }

    /// <summary>
    /// Unity's Awake event function
    /// </summary>
    protected void Update()
    {
      SourceNetworkTransform.transform.position = TargetTransform?.Position ?? Vector3.zero;
      SourceNetworkTransform.transform.rotation = TargetTransform?.Rotation ?? Quaternion.identity;
    }
  }
}