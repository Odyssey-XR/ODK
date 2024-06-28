#nullable enable

namespace ODK.Shared.Transforms
{
  using Unity.Burst;
  using UnityEngine;

  /// <summary>
  /// Default Locomotion service for basic movement and rotation
  /// </summary>
  [BurstCompile]
  public struct DefaultLocomotionService : ILocomotionService
  {
    /// <inheritdoc />
    public Vector3 CalculateGroundPositionDelta(Vector2 direction, Transform? lookDirection, float speed)
    {
      Vector3 forward  = lookDirection?.forward ?? Vector3.one;
      Vector3 right    = lookDirection?.right ?? Vector3.one;
      Vector3 movement = direction.x * right + direction.y * forward;

      movement.y = 0;

      return movement.normalized * (speed * Time.deltaTime);
    }

    /// <inheritdoc />
    public Quaternion CalculateGroundRotationDelta(float direction, float speed)
    {
      Vector3 rotation = new(0, direction, 0);
      return Quaternion.Euler(rotation * speed * Time.deltaTime);
    }
  }
}