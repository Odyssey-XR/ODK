using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using ODK.Roslyn.Extensions;
using ODK.Roslyn.Tickable.SyntaxReceivers;

namespace ODK.Roslyn.ITickable.SourceGenerators
{
  [Generator]
  public class TickableGenerator : ISourceGenerator
  {
    public void Initialize(GeneratorInitializationContext context)
    {
      context.RegisterForSyntaxNotifications(() => new TickableInterfaceSyntaxReceiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
      if (context.SyntaxContextReceiver is not TickableInterfaceSyntaxReceiver receiver)
        return;

      foreach (INamedTypeSymbol classSymbol in receiver.CandidateSymbols)
      {
        if (classSymbol.IsStatic || classSymbol.IsAbstract)
          return;

        IEnumerable<IMethodSymbol> classMethods = classSymbol.GetMembers().OfType<IMethodSymbol>();

        string unityClassSource = GenerateUnityClass(classSymbol, classMethods);
        context.AddSource($"{classSymbol.Name}MonoBehaviour_generated.cs", SourceText.From(unityClassSource, Encoding.UTF8));
      }
    }

    private string GenerateUnityClass(INamedTypeSymbol classSymbol, IEnumerable<IMethodSymbol> classMethods)
    {
      StringBuilder sourceBuilder     = new();
      bool          hasNamespace      = !string.IsNullOrWhiteSpace(classSymbol.ContainingNamespace?.Name ?? "");
      bool          isNetworkTickable = classSymbol.ImplementsTypes("INetworkTickable");
      bool          hasConstructor    = classSymbol.Constructors.Any(_ => _.Parameters.Length > 0);

      string baseUnityClass = isNetworkTickable ? "NetworkBehaviour" : "MonoBehaviour";
      sourceBuilder.AppendLine($"using UnityEngine;\n");

      if (hasNamespace)
      {
        sourceBuilder.Append(
          $$"""
            namespace {{classSymbol.ContainingNamespace!.Name}}
            {           
            
            """);  
      }
      
      sourceBuilder.AppendLine($$"""
                                   [AddComponentMenu("{{classSymbol.Name}}")]
                                   public class {{classSymbol.Name}}MonoBehaviour_generated : {{baseUnityClass}}
                                   {
                                     private {{classSymbol.Name}} _tickable;
                                     
                                 """);

      // We want to leave constructor injection to the injection source generator
      if (hasConstructor)
        sourceBuilder.Append($"    public void Start() {{ _tickable = DependencyInjector.CreateWithDependencies(new {classSymbol.Name}DependencyResolver()); }}\n");
      else
        sourceBuilder.Append($"    public void Start() {{ _tickable = new {classSymbol.Name}(); }}\n");

      if (classMethods.Any(_ => _.Name == "OnEnabled"))
        sourceBuilder.Append("    public void Enabled() { _tickable.OnEnabled(); }\n");

      if (classMethods.Any(_ => _.Name == "OnDisabled"))
        sourceBuilder.Append("    public void Disabled() { _tickable.OnDisabled(); }\n");

      if (classMethods.Any(_ => _.Name == "OnTick"))
        sourceBuilder.Append("    public void Update() { _tickable.OnTick(); }\n");

      if (classMethods.Any(_ => _.Name == "OnFixedTick"))
        sourceBuilder.Append("    public void OnUpdate() { _tickable.OnFixedTick(); }\n");

      sourceBuilder.Append("  }\n");
      if (hasNamespace)
        sourceBuilder.AppendLine("}");

      return sourceBuilder.ToString();
    }
  }
}