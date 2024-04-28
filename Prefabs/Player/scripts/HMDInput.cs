using Plugins.ODK.Scripts.Interfaces.Input;
using UnityEngine;

public partial class HMDInput : IGetTransformInput
{
  public Vector3 GetPosition()
  {
    return HMD.Position.ReadValue<Vector3>();
  }

  public Quaternion GetRotation()
  {
    return HMD.Rotation.ReadValue<Quaternion>();
  }
}