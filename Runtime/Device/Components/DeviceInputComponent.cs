using System;
using System.Collections.Generic;
using ODK.Attributes;
using ODK.Device.Collections;
using ODK.Events.Managers.Interfaces;
using ODK.Injection.Attributes;
using ODK.Interfaces;
using ODK.Netcode.Attributes;
using ODK.Netcode.Enums;
using Unity.Netcode;

namespace ODK.Device.Components
{
  public class DeviceInputComponent : INetworkComponent, IDisposable
  {
    private readonly NetworkBehaviour _parent;
    private readonly IEventManager _eventManager;

    [EditorField]
    public ODK.Device.Enums.DeviceType Device;
    
    [EditorField]
    public List<DeviceInputAction> InputActions = new();

    [NetworkMethod(NetworkUsage.Client)]
    public DeviceInputComponent(
      [Parent] NetworkBehaviour parent,
      IEventManager eventManager)
    {
      _parent = parent;
      if (!_parent.IsClient)
        return;
      
      _eventManager = eventManager;
    }
    
    [NetworkMethod(NetworkUsage.Client)]
    public void Dispose()
    {
      foreach (DeviceInputAction action in InputActions)
        action.Dispose();
    }
  }
}