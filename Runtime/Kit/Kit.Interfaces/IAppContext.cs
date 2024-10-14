using ODK.Injection.Interfaces;

namespace ODK.Kit.Interfaces
{
  public interface IAppContext
  {
    IDependencyContainer Container { get; }
  }
}