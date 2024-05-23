#nullable enable

namespace ODK.Shared.Transforms
{
  using UnityEngine;

  public struct DefaultLocomotionService : ILocomotionService
  {
    public Vector3 CalculateMovementDelta(Vector2 direction, Transform? directionSource)
    {
      Vector3 forward  = directionSource?.forward ?? Vector3.one;
      Vector3 right    = directionSource?.right ?? Vector3.one;
      Vector3 movement = direction.x * right + direction.y * forward;

      movement.y = 0;

      return movement.normalized * Time.deltaTime;
    }

    public Quaternion CalculateRotationDelta(Vector2 direction)
    {
      Vector3 rotation = new(0, direction.x, 0);
      return Quaternion.Euler(rotation * Time.deltaTime);
    }
  }
}