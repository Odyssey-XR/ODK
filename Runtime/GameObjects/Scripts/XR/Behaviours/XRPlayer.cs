#nullable enable

namespace ODK.GameObjects.XR
{
  using Unity.Netcode;
  using UnityEngine;

  public class XRPlayer : NetworkBehaviour
  {
    [SerializeField]
    private GameObject? _leftHand;
    
    [SerializeField]
    private GameObject? _rightHand;
    
    [SerializeField]
    private GameObject? _head;
    
    protected virtual void Start()
    {
      if (!IsOwner)
        return;

      transform.name = $"XR Player: {OwnerClientId}";
      
      Debug.Log($"Setting up xr player(id: {OwnerClientId})");
      
      Camera? sceneCamera = Camera.main;
      if (sceneCamera is null)
      {
        Debug.LogError("XRPlayer requires a main camera in the scene");
        return;
      }

      sceneCamera!.transform.parent = _head?.transform;
      Debug.Log($"Finished setting up xr player(id: {OwnerClientId})");
    }
  }
}