using Unity.Netcode;

namespace ODK.Netcode.Behaviours.Interfaces
{
  public interface IPredictedInput<in T> : INetworkSerializable
  {
    bool ShouldReconcile(T clientInput);
  }
}