using ODK.Netcode.Behaviours.Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace ODK.Locomotion.ODK.Locomotion.Models
{
  [GenerateSerializationForType(typeof(RotationInputModel))]
  public struct RotationInputModel : IPredictedInput<RotationInputModel>
  {
    public Vector3 Rotation;

    public bool ShouldReconcile(RotationInputModel clientInput)
    {
      return Mathf.Abs(clientInput.Rotation.x) != 0 || Mathf.Abs(clientInput.Rotation.y) > 1 || Mathf.Abs(clientInput.Rotation.z) != 0;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref Rotation);
    }
  }
}