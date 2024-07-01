using ODK.Netcode.Behaviours.Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace ODK.Locomotion.ODK.Locomotion.Models
{
  [GenerateSerializationForType(typeof(TransformInputModel))]
  public struct TransformInputModel : IPredictedInput<TransformInputModel>
  {
    public Vector3 Position;
    public Vector3 Rotation;
    
    public bool ShouldReconcile(TransformInputModel clientInput)
    {
      return false;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref Position);
      serializer.SerializeValue(ref Rotation);
    }
  }
}