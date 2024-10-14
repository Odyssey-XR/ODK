#nullable enable
using System;
using ODK.Injection.Interfaces;
using UnityEngine;

namespace ODK.Injection
{
  public class DependencyProvider<TConcrete> : IDependencyProvider 
  {
    private bool _isSingleton = true;
    private Func<TConcrete>? _singletonConstructor;
    private Func<TConcrete>? _transientConstructor;

    private TConcrete? ConstructSingleton()
    {
      return _singletonConstructor is null ? DependencyBuilder.Build<TConcrete>() : _singletonConstructor.Invoke();
    }

    private TConcrete? ConstructTransient()
    {
      return _transientConstructor is null ? DependencyBuilder.Build<TConcrete>() : _transientConstructor.Invoke();
    }

    public IDependencyProvider AsSingleton()
    {
      TConcrete singleton = DependencyBuilder.Build<TConcrete>();
      return AsSingleton(() => singleton);
    }

    public IDependencyProvider AsSingleton<T>(T singletonValue)
    {
      return AsSingleton(() => singletonValue);
    }

    public IDependencyProvider AsSingleton<T>(Func<T> constructor)
    {
      if (typeof(T) != typeof(TConcrete))
        Debug.LogError($"{typeof(T)} does not match {typeof(TConcrete)} in singleton creation");

      _isSingleton          = true;
      _singletonConstructor = constructor as Func<TConcrete>;
      return this;
    }

    public IDependencyProvider AsTransient()
    {
      return AsTransient(DependencyBuilder.Build<TConcrete>);
    }

    public IDependencyProvider AsTransient<T>(Func<T> constructor)
    {
      if (typeof(T) != typeof(TConcrete))
        Debug.LogError($"{typeof(T)} does not match {typeof(TConcrete)} in transient creation");

      _isSingleton          = false;
      _transientConstructor = constructor as Func<TConcrete>;
      return this;
    }

    public TResolved ResolveToTypedValue<TResolved>()
    where TResolved : class
    {
      return ((_isSingleton ? ConstructSingleton() : ConstructTransient()) as TResolved)!;
    }
  }
}