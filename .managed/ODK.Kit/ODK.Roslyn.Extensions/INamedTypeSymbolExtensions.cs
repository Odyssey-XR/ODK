namespace ODK.Roslyn.Extensions;

public static class INamedTypeSymbolExtensions
{
  public static bool ImplementsTypes(this INamedTypeSymbol typeSymbol, bool directInheritance, params string[] types)
  {
    if (typeSymbol is null)
      return false;

    if (types.Contains(typeSymbol.Name))
      return true;
    
    if (typeSymbol.Interfaces.Any(_ => ImplementsTypes(_, directInheritance, types)))
      return true;

    if (!directInheritance)
      return ImplementsTypes(typeSymbol.BaseType!, directInheritance, types);
    return false;
  }

  public static bool ImplementsTypes(this INamedTypeSymbol typeSymbol, params string[] types)
  {
    return ImplementsTypes(typeSymbol, false, types);
  }
}