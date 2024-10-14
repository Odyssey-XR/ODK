namespace ODK.Roslyn.SyntaxReceivers;

public class ClassSyntaxReceiver : ISyntaxContextReceiver
{
  public List<INamedTypeSymbol> CandidateSymbols { get; set; } = [];

  public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
  {
    if (context.Node is not ClassDeclarationSyntax classDeclarationSyntax)
      return;

    if (context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) is INamedTypeSymbol classSymbol)
      CandidateSymbols.Add(classSymbol);
  }
}