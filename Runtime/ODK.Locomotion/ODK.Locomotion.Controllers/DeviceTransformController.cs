#nullable enable
using ODK.Interaction.Controllers.Interfaces;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace ODK.Locomotion.Controllers
{
  public class DeviceTransformController : NetworkBehaviour
  {
    [SerializeField]
    private MonoBehaviour? _deviceTransformInputEventInterface;

    [SerializeField]
    private Transform? _target;

    [SerializeField]
    private bool _isClientAuthoritative;

    private NetworkVariable<Vector3> _devicePosition = new(writePerm: NetworkVariableWritePermission.Owner);
    
    private NetworkVariable<Vector3> _deviceRotation = new(writePerm: NetworkVariableWritePermission.Owner);
    
    private IDeviceTransformInput _deviceTransform => (IDeviceTransformInput)_deviceTransformInputEventInterface!;

    public override void OnNetworkSpawn()
    {
      if (_deviceTransformInputEventInterface is not IDeviceTransformInput)
        Debug.LogError($"Provided behaviour does not resolve to an {nameof(IDeviceTransformInput)}");
    }

    private void Update()
    {
      if (IsClient && IsOwner)
      {
        _devicePosition.Value = _deviceTransform.DevicePosition;
        _deviceRotation.Value = _deviceTransform.DeviceRotation.eulerAngles;
      }
      
      switch (_isClientAuthoritative)
      {
        case false when IsServer:
        case true when IsClient && IsOwner:
          SetTargetTransform(_devicePosition.Value, _deviceRotation.Value);
          break;
      }
    }

    private void SetTargetTransform(Vector3 devicePosition, Vector3 deviceRotation)
    {
      if (_target is null)
        return;

      _target.localPosition = devicePosition;
      _target.localRotation = Quaternion.Euler(deviceRotation);
    }
  }
}