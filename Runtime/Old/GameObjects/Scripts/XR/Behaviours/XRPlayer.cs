#nullable enable

namespace ODK.GameObjects.XR
{
  using ODK.Shared.Player;
  using Unity.Netcode;
  using UnityEngine;

  public class XRPlayer : NetworkBehaviour, IPlayer
  {
    [SerializeField]
    private DevicePhysicalInputEventTracker? _leftHand;

    [SerializeField]
    private DevicePhysicalInputEventTracker? _rightHand;

    [SerializeField]
    private GameObject? _head;

    [SerializeField]
    private Camera? _camera;

    public IInputEventer? PrimaryInput => _leftHand;

    public IInputEventer? SecondaryInput => _rightHand;

    protected virtual void Start()
    {
      transform.name = $"XR Player: {OwnerClientId}";
      Debug.Log($"Setting up xr player(id: {OwnerClientId})");

      SetupPlayer_Server();
      SetupPlayer_Owner();

      Debug.Log($"Finished setting up xr player(id: {OwnerClientId})");
    }

    protected virtual void SetupPlayer_Owner()
    {
      if (_camera is null || !IsOwner)
        return;

      _camera.GetComponent<AudioListener>().enabled = true;
    }

    protected virtual void SetupPlayer_Server()
    {
      if (_camera is null || !IsServer)
        return;

      _camera.GetComponent<AudioListener>().enabled = false;
    }
  }
}