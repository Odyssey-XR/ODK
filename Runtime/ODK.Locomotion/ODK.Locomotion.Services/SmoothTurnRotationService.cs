using ODK.Locomotion.Services.Interfaces;
using UnityEngine;

namespace ODK.Locomotion.Services
{
  public struct SmoothTurnRotationService : IRotationService
  {
    public Vector3 UpdateRotation(GameObject gameObject, Vector3 rotation, float speed)
    {
      gameObject.transform.rotation *= Quaternion.Euler(rotation * (speed * Time.deltaTime));
      return gameObject.transform.eulerAngles;
    }
  }
}