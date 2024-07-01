using Omni.Attributes;
using Unity.Netcode;
using Unity.Netcode.Components;

namespace ODK.Netcode.Prediction
{
  public partial class PredictedNetworkTransform : NetworkBehaviour
  {
    [Component] 
    private NetworkTransform _networkTransform { get; set; }
    
    /// <inheritdoc />
    public override void OnNetworkSpawn()
    {
      ResolveComponents();

      if (!IsServer && IsOwner)
        _networkTransform.enabled = false;
    }
  }
}