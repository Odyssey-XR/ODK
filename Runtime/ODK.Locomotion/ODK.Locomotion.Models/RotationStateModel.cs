using ODK.Netcode.Behaviours.Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace ODK.Locomotion.ODK.Locomotion.Models
{
  [GenerateSerializationForType(typeof(RotationStateModel))]
  public struct RotationStateModel : IPredictedState<RotationStateModel>
  {
    public Vector3 EulerAngles;

    public bool ShouldReconcile(RotationStateModel clientPredicted, RotationStateModel serverPredicted)
    {
      return Vector3.Distance(clientPredicted.EulerAngles, serverPredicted.EulerAngles) >= 50f;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref EulerAngles);
    }
  }
}