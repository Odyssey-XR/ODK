using UnityEngine;

namespace ODK.Locomotion.Services.Interfaces
{
  public interface ILocomotionService
  {
    Vector3 UpdatePosition(Transform transform, Vector3 direction, Vector3 forward, Vector3 right, float speed);
  }
}