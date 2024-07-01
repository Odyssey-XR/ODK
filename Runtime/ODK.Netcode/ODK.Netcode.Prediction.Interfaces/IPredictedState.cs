using Unity.Netcode;

namespace ODK.Netcode.Behaviours.Interfaces
{
  public interface IPredictedState<in T> : INetworkSerializable
  {
    bool ShouldReconcile(T clientPredicted, T serverPredicted);
  }
}