using ODK.Netcode.Behaviours.Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace ODK.Locomotion.ODK.Locomotion.Models
{
  [GenerateSerializationForType(typeof(TransformStateModel))]
  public struct TransformStateModel : IPredictedState<TransformStateModel>
  {
    public Vector3 Position;
    public Vector3 Rotation;

    public bool ShouldReconcile(TransformStateModel clientPredicted, TransformStateModel serverPredicted)
    {
      if (Vector3.Distance(clientPredicted.Position, serverPredicted.Position) >= 10f)
        return true;
      if (Vector3.Distance(clientPredicted.Rotation, serverPredicted.Rotation) >= 10f)
        return true;
      return false;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref Position);
      serializer.SerializeValue(ref Rotation);
    }
  }
}