using System;
using ODK.Device.Enums;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ODK.Device.Collections
{
  [Serializable]
  public class DeviceInputAction : IDisposable
  {
    [SerializeField]
    private InputAction _action;
    
    public string Name;
    public DeviceInterface DeviceInterface; 
    public InputAction Action
    {
      get
      {
        _action?.Enable();
        return _action;
      }
    }

    public TAction ReadValue<TAction>() 
    where TAction : struct
    {
      return Action?.ReadValue<TAction>() ?? default;
    }
    
    public bool TryReadValue<TAction>(out TAction value) 
    where TAction : struct
    {
      try
      {
        value = Action?.ReadValue<TAction>() ?? default;
        return true;
      }
      catch (Exception ex)
      {
        Debug.LogError(ex);
        value = default;
        return false;
      }
    }
    
    public void Dispose()
    {
      _action?.Disable();
      _action?.Dispose();
    }
  }
}