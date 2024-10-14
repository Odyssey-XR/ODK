using System.Linq;

namespace ODK.Roslyn.SyntaxReceivers;

public abstract class BaseTypeSyntaxReceiver : ISyntaxContextReceiver
{
  public List<INamedTypeSymbol> CandidateSymbols { get; set; } = [];
  
  private readonly bool _directInheritance;
  private readonly string[] _types;

  public BaseTypeSyntaxReceiver(params string[] types)
  {
    _types = types;
  }

  public BaseTypeSyntaxReceiver(bool directInheritance, params string[] types)
  {
    _directInheritance = directInheritance;
    _types             = types;
  }
  
  public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
  {
    if (context.Node is not ClassDeclarationSyntax classDeclarationSyntax)
      return;
    
    INamedTypeSymbol? classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) as INamedTypeSymbol;
    
    if (classSymbol is not null && classSymbol.ImplementsTypes(_types))
      CandidateSymbols.Add(classSymbol);
  }
}