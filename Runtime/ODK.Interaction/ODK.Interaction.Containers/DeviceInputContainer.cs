using ODK.Interaction.Controllers.Interfaces;
using ODK.Interaction.Services;
using ODK.Interaction.Services.Interfaces;
using ODK.XR.Interaction.Controllers;
using Omni.Providers;
using UnityEngine;

namespace ODK.Interaction.Containers
{
  public class DeviceInputContainer : NetworkContainer
  {
    [SerializeField]
    private DeviceInputController _primaryInputController;

    [SerializeField]
    private DeviceInputController _secondaryInputController;

    protected override void OnBind(ContainerProvider container)
    {
      container.Bind<IDeviceInputReaderService, DeviceInputReaderService>();
      container.Bind<IDeviceInterfaceInputConsumerService, DeviceInterfaceInputConsumerService>();
    }

    protected override void OnOwnerBind(ContainerProvider container)
    {
      container.Bind<IPrimaryDeviceInputController, DeviceInputController>().AsSingleton(_primaryInputController);
      container.Bind<ISecondaryDeviceInputController, DeviceInputController>().AsSingleton(_secondaryInputController);
    }
  }
}