using ODK.Injection.Attributes;

namespace ODK.Roslyn.Injection.Analyzers
{
  [DiagnosticAnalyzer(LanguageNames.CSharp)]
  public class ParentAttributeUsageAnalyzer : DiagnosticAnalyzer
  {
    private static readonly DiagnosticDescriptor Rule = new(
      id: "PA001",
      title: "Parameter with ParentAttribute must be of type MonoBehaviour",
      messageFormat: "Parameter '{0}' with ParentAttribute must be of type MonoBehaviour",
      category: "TypeCheck",
      DiagnosticSeverity.Error,
      isEnabledByDefault: true);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
      context.EnableConcurrentExecution();
      context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

      context.RegisterSyntaxNodeAction(AnalyzeParameter, SyntaxKind.Parameter);
    }

    private static void AnalyzeParameter(SyntaxNodeAnalysisContext context)
    {
      ParameterSyntax parameterSyntax = (ParameterSyntax)context.Node;
      bool hasParentAttribute = parameterSyntax.AttributeLists
        .SelectMany(a => a.Attributes)
        .Any(a => ModelExtensions.GetSymbolInfo(context.SemanticModel, a).Symbol is IMethodSymbol methodSymbol &&
                  methodSymbol.ContainingType.Name == nameof(ParentAttribute));

      if (!hasParentAttribute)
        return;

      ITypeSymbol parameterType = ModelExtensions.GetTypeInfo(context.SemanticModel, parameterSyntax.Type).Type;
      if (parameterType is null)
        return;

      if (parameterType.ImplementsTypes("MonoBehaviour")) 
        return;
      
      Diagnostic diagnostic = Diagnostic.Create(Rule, parameterSyntax.GetLocation(), parameterSyntax.Identifier.Text);
      context.ReportDiagnostic(diagnostic);
    }
  }
}