namespace ODK.Shared.Behaviours
{
  using Unity.Netcode.Components;
  using UnityEngine;

  /// <summary>
  /// Used for syncing a transform across the network from a network owner that might
  /// exist on the server or a local client
  /// </summary>
  [DisallowMultipleComponent]
  public class OwnerNetworkTransform : NetworkTransform
  {
    /// <inheritdoc />
    protected override bool OnIsServerAuthoritative()
    {
      return IsServer || IsOwner;
    }
  }
}