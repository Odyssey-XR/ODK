using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using ODK.Roslyn.Extensions;
using ODK.Roslyn.Tickable.SyntaxReceivers;

namespace ODK.Roslyn.Tickable.SourceGenerators
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
        context.AddSource($"{classSymbol.ToDisplayString()}.g.cs", SourceText.From(unityClassSource, Encoding.UTF8));
      }
    }

    private string GenerateUnityClass(INamedTypeSymbol classSymbol, IEnumerable<IMethodSymbol> classMethods)
    {
      StringBuilder sourceBuilder     = new();
      bool          isNetworkTickable = classSymbol.ImplementsTypes("NetworkTickable");
      bool          hasConstructor    = classSymbol.Constructors.Any();

      string baseUnityClass = isNetworkTickable ? "NetworkBehaviour" : "MonoBehaviour";
      sourceBuilder.Append(
        $$"""
        using UnityEngine;
        
        namespace ODK.Roslyn.Tickable.SourceGenerators
        {
          public partial class {{classSymbol.Name}} : {{baseUnityClass}}
          {
            private readonly {{classSymbol.Name}} _tickable; "
          
        """);

      // We want to leave constructor injection to the injection source generator
      if (hasConstructor)
      {
        sourceBuilder.Append($$"""
                                   public void Start() { _tickable = DependencyInjector.CreateWithDependencies(new {{classSymbol.Name}}DependencyResolver()); }  
                               """);
      }
      else
      {
        sourceBuilder.Append($$"""
                                   public void Start() { _tickable = new {{classSymbol.Name}}(); }
                               """);
      }

      if (classMethods.Any(_ => _.Name == "OnEnabled"))
        sourceBuilder.Append("public void Enabled() { _tickable.OnEnabled(); }");
      
      if (classMethods.Any(_ => _.Name == "OnDisabled"))
        sourceBuilder.Append("public void Disabled() { _tickable.OnDisabled(); }");
      
      if (classMethods.Any(_ => _.Name == "OnTick"))
        sourceBuilder.Append("public void Update() { _tickable.OnTick(); }");
      
      if (classMethods.Any(_ => _.Name == "OnFixedTick"))
        sourceBuilder.Append("public void OnUpdate() { _tickable.OnFixedTick(); }");
      
      sourceBuilder.Append(
        """
          }
        }
        """);
      
      return sourceBuilder.ToString();
    }
  }
}