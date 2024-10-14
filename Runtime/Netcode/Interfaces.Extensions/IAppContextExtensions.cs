using ODK.Kit.Interfaces;
using ODK.Netcode.Managers;
using ODK.Netcode.Managers.Interfaces;
using ODK.Netcode.Services;
using ODK.Netcode.Services.Interfaces;

namespace ODK.Netcode.Interfaces.Extensions
{
  public static class IAppContextExtensions
  {
    public static IAppContext UseNetworking(this IAppContext context)
    {
      context.Container.Bind<INetworkManager, NetworkManager>();
      context.Container.Bind<ILobbyService, LobbyService>();
      return context;
    }
  }
}