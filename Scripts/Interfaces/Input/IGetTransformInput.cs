#nullable enable

namespace Plugins.ODK.Scripts.Interfaces.Input
{
  using UnityEngine;

  public interface IGetTransformInput
  {
    Vector3    GetPosition();
    Quaternion GetRotation();
  }
}