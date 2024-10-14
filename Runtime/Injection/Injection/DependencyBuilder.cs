using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ODK.Injection
{
  public static class DependencyBuilder
  {
    private static readonly Dictionary<Type, Func<object>> _buildMethods = new();

    static DependencyBuilder()
    {
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      IEnumerable<MethodInfo> resolverMethods = assemblies.SelectMany(assembly => assembly.GetTypes())
        .Where(type => type.Name.EndsWith("DependencyResolver"))
        .Select(type => type.GetMethod("CreateWithDependencies", BindingFlags.Static | BindingFlags.Public))
        .Where(method => method != null);

      foreach (MethodInfo method in resolverMethods)
      {
        Type returnType = method.ReturnType;
        _buildMethods[returnType] = () => method.Invoke(null, null);
      }
    }

    public static T Build<T>()
    {
      if (_buildMethods.TryGetValue(typeof(T), out Func<object> buildMethod))
        return (T)buildMethod();

      throw new InvalidOperationException($"No build method registered for type {typeof(T).FullName}");
    }
  }
}