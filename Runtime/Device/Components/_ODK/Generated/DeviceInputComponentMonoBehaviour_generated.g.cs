/*
 * This class is automatically generated. 
 * Any changes made to this file will be overwritten.
*/
 
#pragma warning disable   
#nullable enable

using System;
using UnityEngine;
      
namespace ODK.Generated
{           
  [AddComponentMenu("DeviceInput")]
  public class DeviceInputComponentMonoBehaviour_generated : Unity.Netcode.NetworkBehaviour
  {
    [SerializeField] public ODK.Device.Enums.DeviceType Device;
    [SerializeField] public System.Collections.Generic.List<ODK.Device.Collections.DeviceInputAction> InputActions;

    private ODK.Device.Components.DeviceInputComponent _component;
      
    public void Start() 
    { 
      if (Device == null)
        throw new Exception("Device cannot be null");
      if (InputActions == null)
        throw new Exception("InputActions cannot be null");

      _component = DeviceInputComponentDependencyResolver.CreateWithDependencies(this);
      _component.Device = Device;
      _component.InputActions = InputActions;
    
      
}

    public void OnDestroy() 
    {
      if (_component is IDisposable disposable)
        disposable.Dispose();
    }
  }
}
#pragma warning restore