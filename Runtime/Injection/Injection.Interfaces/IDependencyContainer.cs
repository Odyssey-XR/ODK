#nullable enable  

namespace ODK.Injection.Interfaces
{
  public interface IDependencyContainer
  {
    IDependencyProvider Bind<TInterface, TConcrete>()
    where TInterface : class
    where TConcrete : TInterface;

    public TInterface? Resolve<TInterface>()
    where TInterface : class;
  }
}