using ODK.Locomotion.Services.Interfaces;
using Unity.Burst;
using UnityEngine;

namespace ODK.Locomotion.Services
{
  [BurstCompile]
  public struct LinearLocomotionService : ILocomotionService
  {
    public Vector3 UpdatePosition(GameObject gameObject, Vector3 direction, Vector3 forward, Vector3 right, float speed)
    {
      direction = Vector3.Normalize(direction);
      Vector3 delta = direction.x * right + direction.y * Vector3.one + direction.z * forward;
      gameObject.transform.position += delta * (speed * Time.deltaTime);

      return gameObject.transform.position;
    }
  }
}