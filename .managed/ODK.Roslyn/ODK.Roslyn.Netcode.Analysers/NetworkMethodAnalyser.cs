namespace ODK.Roslyn.Netcode.Analysers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class NetworkMethodAnalyzer : DiagnosticAnalyzer
{
  public const string ContextMismatchId = "NetworkMethodContextMismatch";
  public const string MissingCheckId = "NetworkMethodMissingCheck";
  public const string MissingAttributeId = "MonoBehaviourMethodMissingNetworkAttribute";

  private static readonly LocalizableString ContextMismatchTitle =
    "Network method context mismatch";

  private static readonly LocalizableString ContextMismatchMessageFormat =
    "Method '{0}' with {1} context is called from a {2} context method";

  private static readonly LocalizableString MissingCheckTitle =
    "Missing network context check";

  private static readonly LocalizableString MissingCheckMessageFormat =
    "Method '{0}' with {1} context is missing the appropriate context check";

  private static readonly LocalizableString MissingAttributeTitle =
    "MonoBehaviour method missing NetworkMethodAttribute";

  private static readonly LocalizableString MissingAttributeMessageFormat =
    "MonoBehaviour method '{0}' is missing the NetworkMethodAttribute";

  private const string Category = "Usage";

  private static readonly DiagnosticDescriptor ContextMismatchRule = new(
    ContextMismatchId,
    ContextMismatchTitle,
    ContextMismatchMessageFormat,
    Category,
    DiagnosticSeverity.Warning,
    isEnabledByDefault: true);

  private static readonly DiagnosticDescriptor MissingCheckRule = new(
    MissingCheckId,
    MissingCheckTitle,
    MissingCheckMessageFormat,
    Category,
    DiagnosticSeverity.Warning,
    isEnabledByDefault: true);

  private static readonly DiagnosticDescriptor MissingAttributeRule = new(
    MissingAttributeId,
    MissingAttributeTitle,
    MissingAttributeMessageFormat,
    Category,
    DiagnosticSeverity.Warning,
    isEnabledByDefault: true);

  public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
    ImmutableArray.Create(ContextMismatchRule, MissingCheckRule, MissingAttributeRule);

  public override void Initialize(AnalysisContext context)
  {
    context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
    context.EnableConcurrentExecution();
    context.RegisterSyntaxNodeAction(AnalyzeMethodDeclaration, SyntaxKind.MethodDeclaration);
    context.RegisterSyntaxNodeAction(AnalyzeInvocation, SyntaxKind.InvocationExpression);
  }

  private static void AnalyzeMethodDeclaration(SyntaxNodeAnalysisContext context)
  {
    MethodDeclarationSyntax methodDeclaration = (MethodDeclarationSyntax)context.Node;
    IMethodSymbol?          methodSymbol      = context.SemanticModel.GetDeclaredSymbol(methodDeclaration);

    if (methodSymbol is null || methodSymbol.DeclaredAccessibility == Accessibility.Private || !IsNetworkMethod(methodSymbol))
      return;

    // If the method is an RPC method then we already know in which network context it will be called
    bool hasRpcAttribute = methodSymbol
      .GetAttributes()
      .Any(attr => attr.AttributeClass?.Name.Contains("RpcAttribute") ?? false);

    if (hasRpcAttribute)
      return;

    // Get the network method attribute and parse its data members
    AttributeData? attribute = methodSymbol
      .GetAttributes()
      .FirstOrDefault(attr => attr.AttributeClass?.Name == "NetworkMethodAttribute");

    if (attribute is null)
    {
      context.ReportDiagnostic(Diagnostic.Create(
        MissingAttributeRule,
        methodDeclaration.Identifier.GetLocation(),
        methodSymbol.Name));
      return;
    }

    if (!int.TryParse(attribute.ConstructorArguments[0].Value?.ToString(), out int contextArg))
      throw new Exception("Unable to parse NetCode context argument");

    bool           forceCheckArg    = attribute.ConstructorArguments[1].Value?.ToString() == "true";
    NetworkContext executionContext = (NetworkContext)contextArg;

    // Don't check for 'correct' context checks if the attribute flag is set to false
    if (!forceCheckArg)
      return;

    if (HasAppropriateContextCheck(methodDeclaration, executionContext))
      return;

    context.ReportDiagnostic(Diagnostic.Create(
      MissingCheckRule,
      methodDeclaration.Identifier.GetLocation(),
      methodSymbol.Name,
      executionContext));
  }

  private static void AnalyzeInvocation(SyntaxNodeAnalysisContext context)
  {
    InvocationExpressionSyntax invocation = (InvocationExpressionSyntax)context.Node;

    if (context.SemanticModel.GetSymbolInfo(invocation.Expression).Symbol is not IMethodSymbol targetMethodSymbol)
      return;

    AttributeData? targetAttribute = targetMethodSymbol
      .GetAttributes()
      .FirstOrDefault(attr => attr.AttributeClass?.Name == "NetworkMethodAttribute");

    if (targetAttribute is null)
      return;

    if (!int.TryParse(targetAttribute.ConstructorArguments[0].Value?.ToString(), out int targetContextArg))
      throw new Exception("Unable to parse NetCode context argument");
    NetworkContext targetContext = (NetworkContext)targetContextArg;

    MethodDeclarationSyntax? currentMethod = invocation
      .Ancestors()
      .OfType<MethodDeclarationSyntax>()
      .FirstOrDefault();
    if (currentMethod is null)
      return;

    IMethodSymbol? currentMethodSymbol = context.SemanticModel.GetDeclaredSymbol(currentMethod);
    AttributeData? currentAttribute = currentMethodSymbol?
      .GetAttributes()
      .FirstOrDefault(attr => attr.AttributeClass?.Name == "NetworkMethodAttribute");

    if (currentAttribute is null)
      return;

    // We might not have a current context value so we can just return in that case as there's nothing to compare
    if (!int.TryParse(currentAttribute.ConstructorArguments[0].Value?.ToString(), out int currentContextArg))
      return;
    NetworkContext currentContext = (NetworkContext)currentContextArg;
        
    if (currentContext.CanTransitionTo(targetContext))
      return;

    Diagnostic diagnostic = Diagnostic.Create(
      ContextMismatchRule,
      invocation.GetLocation(),
      targetMethodSymbol.Name,
      targetContext,
      currentContext);
    context.ReportDiagnostic(diagnostic);
  }

  private static bool IsNetworkMethod(IMethodSymbol methodSymbol)
  {
    INamedTypeSymbol containingType = methodSymbol.ContainingType;
    return containingType.ImplementsTypes(false, "NetworkBehaviour", "INetworkTickable", "IGhostTickable");
  }

  private static bool HasAppropriateContextCheck(
    MethodDeclarationSyntax methodDeclaration,
    NetworkContext executionContext)
  {
    if (executionContext == NetworkContext.Everyone)
      return true;

    if (methodDeclaration.Body?.Statements.FirstOrDefault() is not IfStatementSyntax firstStatement)
      return false;

    string condition = firstStatement.Condition.ToString();
    return executionContext switch
    {
      NetworkContext.Server => condition.Contains("IsServer"),
      NetworkContext.Client => condition.Contains("IsClient"),
      NetworkContext.Owner  => condition.Contains("IsOwner"),
      _                     => true
    };
  }
}