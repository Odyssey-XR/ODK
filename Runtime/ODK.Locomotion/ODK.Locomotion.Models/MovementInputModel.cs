using ODK.Netcode.Behaviours.Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace ODK.Locomotion.ODK.Locomotion.Models
{
  [GenerateSerializationForType(typeof(MovementInputModel))]
  public struct MovementInputModel : IPredictedInput<MovementInputModel>
  {
    public Vector3 Direction;
    public Vector3 Forward;
    public Vector3 Right;
    
    public bool ShouldReconcile(MovementInputModel clientInput)
    {
      return Mathf.Abs(clientInput.Direction.x) > 1 || clientInput.Direction.y != 0 || Mathf.Abs(clientInput.Direction.z) > 1;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref Direction);
      serializer.SerializeValue(ref Forward);
      serializer.SerializeValue(ref Right);
    }
  }
}