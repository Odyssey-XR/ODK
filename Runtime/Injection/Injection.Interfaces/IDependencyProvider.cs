using System;

namespace ODK.Injection.Interfaces
{
  public interface IDependencyProvider
  {
    IDependencyProvider AsSingleton();
    IDependencyProvider AsSingleton<T>(T singletonValue);
    IDependencyProvider AsSingleton<T>(Func<T> constructor);
    IDependencyProvider AsTransient();
    IDependencyProvider AsTransient<T>(Func<T> constructor);
    T ResolveToTypedValue<T>() where T : class;
  }
}