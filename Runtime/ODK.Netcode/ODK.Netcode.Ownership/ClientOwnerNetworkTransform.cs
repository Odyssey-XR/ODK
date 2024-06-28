using Unity.Netcode.Components;

namespace ODK.Netcode.Ownership
{
  public class ClientOwnerNetworkTransform : NetworkTransform
  {
    /// <inheritdoc />
    protected override bool OnIsServerAuthoritative()
    {
      return false;
    }
  }
}