using UnityEditor;
using UnityEngine;

namespace ODK.Interaction.Models
{
  public struct TransformInputModel
  {
    public Vector3 DevicePosition;
    public Quaternion DeviceRotation;
    public Vector3 PointerPosition;
    public Quaternion PointerRotation;

    public Vector3 GetPointerForward()
    {
      return PointerRotation * Vector3.forward;
    }

    public Vector3 GetPointerRight()
    {
      return PointerRotation * Vector3.right;
    }

    public Vector3 GetPointerUp()
    {
      return PointerRotation * Vector3.up;
    }
  }
}