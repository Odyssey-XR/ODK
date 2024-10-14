using ODK.Events.Managers;
using ODK.Events.Managers.Interfaces;
using ODK.Kit.Interfaces;

namespace ODK.Events.Interfaces.Extensions
{
  public static class IAppContextExtensions
  {
    public static IAppContext UseEvents(this IAppContext context)
    {
      context.Container.Bind<IEventManager, EventManager>().AsSingleton();
      return context;
    }
  }
}