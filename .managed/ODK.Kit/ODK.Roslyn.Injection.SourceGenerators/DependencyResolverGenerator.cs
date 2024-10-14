using ODK.Injection.Attributes;
using ODK.Interfaces;

namespace ODK.Roslyn.Injection.SourceGenerators;

[Generator]
public class DependencyResolverGenerator : ISourceGenerator
{
  /// <inheritdoc/>
  public void Initialize(GeneratorInitializationContext context)
  {
    context.RegisterForSyntaxNotifications(() => new ClassSyntaxReceiver());
  }

  /// <inheritdoc/>
  public void Execute(GeneratorExecutionContext context)
  {
    if (context.SyntaxContextReceiver is not ClassSyntaxReceiver receiver)
      return;

    IEnumerable<INamedTypeSymbol> classSymbols = receiver.CandidateSymbols
      .Where(_ => !_.IsStatic && !_.IsAbstract && !_.IsGenericType && !_.ImplementsTypes("MonoBehaviour", "Attribute"));

    foreach (INamedTypeSymbol classSymbol in classSymbols)
    {
      string source = GenerateDependencyResolver(classSymbol);
      context.AddSource($"{classSymbol.Name}DependencyResolver.g.cs", source);
    }
  }

  public string GenerateDependencyResolver(INamedTypeSymbol classSymbol)
  {
    bool isNetworkTickable = classSymbol.ImplementsTypes(nameof(INetworkComponent)); 
    string className = classSymbol.Name;
    string resolverClassName = $"{className}DependencyResolver";
    string parentClass = isNetworkTickable ? "Unity.Netcode.NetworkBehaviour" : "MonoBehaviour";   

    // Get the constructor and its parameters
    IMethodSymbol? constructor = classSymbol.Constructors
      .FirstOrDefault(_ => !_.IsStatic && _.DeclaredAccessibility == Accessibility.Public);

    if (constructor is null)
      return string.Empty;

    ImmutableArray<IParameterSymbol> constructorParams = constructor.Parameters;
    bool hasParentAttribute = constructorParams.Any(param =>
      param.GetAttributes().Any(attr =>
        attr.AttributeClass?.Name == nameof(ParentAttribute)));
    
    // Collect all namespaces for the parameter types
    HashSet<string> namespaces = [];
    if (!string.IsNullOrWhiteSpace(classSymbol.ContainingNamespace.Name))
      namespaces.Add(classSymbol.ContainingNamespace.ToDisplayString());

    foreach (IParameterSymbol? param in constructorParams)
    {
      string namespaceSymbol = param.Type.ContainingNamespace.Name;
      if (!string.IsNullOrEmpty(namespaceSymbol))
        namespaces.Add(param.Type.ContainingNamespace.ToDisplayString());
    }

    if (hasParentAttribute)
      namespaces.Add("UnityEngine");

    // Generate the source code for dependency resolution list
    string dependencyResolutionList = string.Join(
      "\n",
      constructorParams
        .Where(_ => !_.GetAttributes().Any(attr => attr.AttributeClass?.Name == nameof(ParentAttribute)))
        .Select(_ => $"         {_.Type.Name} {_.Name} = DependencyContainer._Resolve<{_.Type.Name}>();"));

    if (hasParentAttribute)
      dependencyResolutionList += $"         \n{parentClass} parent = __parent;";

    // Prepare constructor argument list
    IEnumerable<string> constructorArgumentList = constructorParams.Select(_ => _.Name);

    // Generate using directives
    string usingDirectives = string.Join("\n", namespaces.Select(_ => $"using {_};"));

    return $$"""
             #pragma warning disable
             
             using ODK.Injection;
             {{usingDirectives}}

             public static class {{resolverClassName}}
             {
                 public static {{className}} CreateWithDependencies({{(hasParentAttribute ? $"{parentClass} __parent" : "")}})
                 {
                     {{dependencyResolutionList}}
                     return new {{className}}({{string.Join(", ", constructorArgumentList)}});
                 }
             }
             #pragma warning restore 
             """;
  }
}