using Plugins.ODK.Scripts.Interfaces.Input;
using UnityEngine;

public partial class RightControllerInput : IGetTransformInput
{
  public Vector3    GetPosition()
  {
    return RightController.Position.ReadValue<Vector3>();
  }

  public Quaternion GetRotation()
  {
    return RightController.Rotation.ReadValue<Quaternion>();
  }
}