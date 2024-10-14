#nullable enable
using System;
using System.Collections.Generic;
using ODK.Injection.Interfaces;

namespace ODK.Injection
{
  public class DependencyContainer : IDependencyContainer
  {
    public static Dictionary<Type, IDependencyProvider> Providers { get; } = new();

    public static IDependencyProvider _Bind<TInterface, TConcrete>()
    where TInterface : class
    where TConcrete : TInterface
    {
      if (Providers.ContainsKey(typeof(TInterface)))
        throw new Exception("Dependency provider already registered");

      DependencyProvider<TConcrete> dependencyProvider = new();
      Providers[typeof(TInterface)] = dependencyProvider;
      return dependencyProvider;
    }

    public static TInterface? _Resolve<TInterface>()
    where TInterface : class
    {
      if (Providers.ContainsKey(typeof(TInterface)))
        return Providers[typeof(TInterface)].ResolveToTypedValue<TInterface>();

      throw new Exception($"Dependency {typeof(TInterface).Name} does not exist in container");
    }

    public IDependencyProvider Bind<TInterface, TConcrete>()
    where TInterface : class
    where TConcrete : TInterface
    {
      return _Bind<TInterface, TConcrete>();
    }

    public TInterface? Resolve<TInterface>()
    where TInterface : class
    {
      return _Resolve<TInterface>();
    }
  }
}