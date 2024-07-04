using UnityEngine;

namespace ODK.Locomotion.Services.Interfaces
{
  public interface IRotationService
  {
    Vector3 UpdateRotation(GameObject gameObject, Vector3 rotation, float speed);
  }
}