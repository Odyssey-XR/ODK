using Omni.Attributes;
using Unity.Netcode;
using UnityEngine;

namespace ODK.XR.Interaction.Controllers
{
  public partial class CameraInitializationController : NetworkBehaviour
  {
    [Component]
    private Camera _camera { get; set; }

    [Component]
    private AudioListener _audioListener { get; set; }

    public override void OnNetworkSpawn()
    {
      ResolveComponents();
      
      if (IsOwner)
      {
        _camera.enabled        = true;
        _audioListener.enabled = true;
      }
      else
      {
        _camera.enabled        = false;
        _audioListener.enabled = false;
      }
    }
  }
}