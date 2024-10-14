using ODK.Injection;
using ODK.Injection.Interfaces;
using ODK.Kit.Interfaces;

namespace ODK.Kit
{
  public class AppContext : IAppContext
  {
    public IDependencyContainer Container { get; } = new DependencyContainer();
  }
}