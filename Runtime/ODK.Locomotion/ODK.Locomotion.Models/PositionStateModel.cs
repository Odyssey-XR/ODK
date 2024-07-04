using ODK.Netcode.Behaviours.Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace ODK.Locomotion.ODK.Locomotion.Models
{
  [GenerateSerializationForType(typeof(PositionStateModel))]
  public struct PositionStateModel : IPredictedState<PositionStateModel>
  {
    public Vector3 Position;

    public bool ShouldReconcile(PositionStateModel clientPredicted, PositionStateModel serverPredicted)
    {
      return Vector3.Distance(clientPredicted.Position, serverPredicted.Position) >= 50f;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref Position);
    }
  }
}