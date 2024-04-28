using Plugins.ODK.Scripts.Interfaces.Input;
using UnityEngine;

public partial class LeftControllerInput : IGetTransformInput
{
  public Vector3 GetPosition()
  {
    return LeftController.Position.ReadValue<Vector3>();
  }

  public Quaternion GetRotation()
  {
    return LeftController.Rotation.ReadValue<Quaternion>();
  }
}