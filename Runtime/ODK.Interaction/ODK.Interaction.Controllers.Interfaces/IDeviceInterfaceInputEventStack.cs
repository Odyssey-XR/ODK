using System;

namespace ODK.Interaction.Controllers.Interfaces
{
  public interface IDeviceInterfaceInputEventStack
  {
    void ConnectToInterfaceInputEventStack(Action<IDeviceInterfaceInput> listener);
    void DisconnectFromInterfaceInputEventStack(Action<IDeviceInterfaceInput> listener);
  }
}