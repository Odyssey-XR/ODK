using ODK.Locomotion.Services.Interfaces;
using Unity.Burst;
using UnityEngine;

namespace ODK.Locomotion.Services
{
  [BurstCompile]
  public struct XRLinearLocomotionService : ILocomotionService
  {
    public Vector3 UpdatePosition(Transform transform, Vector3 direction, Vector3 forward, Vector3 right, float speed)
    {
      direction = Vector3.Normalize(direction);
      Vector3 delta = direction.x * right + direction.z * forward + direction.y * Vector3.one;
      return transform.position + delta * speed;
    }
  }
}