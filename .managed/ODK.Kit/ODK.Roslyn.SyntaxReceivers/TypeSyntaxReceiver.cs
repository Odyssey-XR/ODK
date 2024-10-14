namespace ODK.Roslyn.SyntaxReceivers;

public class TypeSyntaxReceiver : ISyntaxContextReceiver
{
  public List<INamedTypeSymbol> CandidateSymbols { get; set; } = [];
  
  private readonly bool _directInheritance;
  private readonly string[] _types;

  public TypeSyntaxReceiver(params string[] types)
  {
    _types = types;
  }

  public TypeSyntaxReceiver(bool directInheritance, params string[] types)
  {
    _directInheritance = directInheritance;
    _types             = types;
  }
  
  public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
  {
    if (context.Node is not ClassDeclarationSyntax classDeclarationSyntax)
      return;
    
    INamedTypeSymbol? classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) as INamedTypeSymbol;
    
    if (classSymbol is not null && classSymbol.ImplementsTypes(_directInheritance, _types))
      CandidateSymbols.Add(classSymbol);
  }
}