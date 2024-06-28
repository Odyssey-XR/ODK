using ODK.Interaction.Controllers.Interfaces;
using ODK.Interaction.Services;
using ODK.Interaction.Services.Interfaces;
using ODK.XR.Interaction.Controllers;
using Omni.Providers;
using UnityEngine;

namespace ODK.Interaction.Containers
{
  public class DeviceInputContainer : ContainerProvider
  {
    [SerializeField] 
    private DeviceInputController _primaryInputController;
    
    [SerializeField] 
    private DeviceInputController _secondaryInputController;

    [SerializeField]
    private DeviceInputController _hmdInputController;
    
    private void Awake()
    {
      Bind<IDeviceInputReaderService, DeviceInputReaderService>();
      Bind<IDeviceInterfaceInputConsumerService, DeviceInterfaceInputConsumerService>();
      LocalBind<IPrimaryDeviceInputController, DeviceInputController>().AsSingleton(_primaryInputController);
      LocalBind<ISecondaryDeviceInputController, DeviceInputController>().AsSingleton(_secondaryInputController);
      LocalBind<IDevicePointerInput, DeviceInputController>().AsSingleton(_hmdInputController);
    }
  }
}