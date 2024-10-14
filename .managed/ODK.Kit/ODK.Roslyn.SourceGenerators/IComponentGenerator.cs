using ODK.Attributes;
using ODK.Injection.Attributes;

namespace ODK.Roslyn.SourceGenerators;

[Generator]
public class IComponentGenerator : ISourceGenerator
{
  private const string CacheDirectoryName = "Cache/ODK";
  private string CacheDirectoryPath => Path.Combine(Directory.GetCurrentDirectory(), CacheDirectoryName);

  /// <inheritdoc/>
  public void Initialize(GeneratorInitializationContext context)
  {
    context.RegisterForSyntaxNotifications(() => new TypeSyntaxReceiver(nameof(IComponent)));
  }

  /// <inheritdoc/>
  public void Execute(GeneratorExecutionContext context)
  {
    if (context.SyntaxContextReceiver is not TypeSyntaxReceiver receiver)
      return;

    IEnumerable<INamedTypeSymbol> classSymbols = receiver.CandidateSymbols
      .Where(_ => !_.IsStatic && !_.IsAbstract && !_.IsGenericType && !_.ImplementsTypes("MonoBehaviour"));

    string assemblyName = context.Compilation.AssemblyName;
    string assemblyCacheFilePath = Path.Combine(CacheDirectoryPath, $"{assemblyName}_GeneratedFilesCache.txt");

    List<string> oldGeneratedFiles =
      File.Exists(assemblyCacheFilePath) ? File.ReadAllLines(assemblyCacheFilePath).ToList() : [];
    List<string> newGeneratedFilePaths = [];

    foreach (INamedTypeSymbol classSymbol in classSymbols)
    {
      if (classSymbol.IsStatic || classSymbol.IsAbstract)
        continue;

      IEnumerable<IMethodSymbol> classMethods = classSymbol.GetMembers().OfType<IMethodSymbol>();

      string? sourceFilePath = classSymbol.Locations.First().SourceTree?.FilePath;
      if (string.IsNullOrWhiteSpace(sourceFilePath))
        continue;

      string? directoryPath = Path.GetDirectoryName(sourceFilePath);
      if (string.IsNullOrWhiteSpace(directoryPath))
        continue;

      string generatedFolder = Path.Combine(directoryPath, "_ODK/Generated");
      Directory.CreateDirectory(generatedFolder);

      string unityClassSource = GenerateUnityClass(classSymbol, classMethods);
      string generatedFilePath = Path.Combine(generatedFolder, $"{classSymbol.Name}MonoBehaviour_generated.g.cs");

      File.WriteAllText(generatedFilePath, unityClassSource);
      newGeneratedFilePaths.Add(generatedFilePath);
    }

    DeleteObsoleteFiles(oldGeneratedFiles, newGeneratedFilePaths);
    UpdateCacheWithGeneratedFiles(assemblyCacheFilePath, newGeneratedFilePaths);
  }

  private void DeleteObsoleteFiles(List<string> oldGeneratedFiles, List<string> newGeneratedFiles)
  {
    foreach (string filePath in oldGeneratedFiles.Except(newGeneratedFiles))
    {
      if (File.Exists(filePath))
        File.Delete(filePath);

      string[] splitPath = filePath.Split('.');
      string metaFilePath = string.Join("", splitPath.Take(splitPath.Length - 1)) + ".meta";
      if (File.Exists(metaFilePath))
        File.Delete(metaFilePath);
    }
  }

  private void UpdateCacheWithGeneratedFiles(string assemblyCacheFilePath, List<string> newGeneratedFilePaths)
  {
    Directory.CreateDirectory(CacheDirectoryPath); // Ensure cache directory exists
    File.WriteAllLines(assemblyCacheFilePath, newGeneratedFilePaths);
  }

  /// <summary>
  /// Takes a named symbol of a class or struct that implements the IComponent interface and generates a corresponding
  /// MonoBehaviour class.
  /// </summary>
  private string GenerateUnityClass(INamedTypeSymbol classSymbol, IEnumerable<IMethodSymbol> classMethods)
  {
    StringBuilder sourceBuilder = new();
    bool isNetworkTickable = classSymbol.ImplementsTypes(nameof(INetworkComponent));
    string classNamespace = classSymbol.ContainingNamespace.ToDisplayString();

    string className = classSymbol.Name;
    string fullClassName = $"{classNamespace}.{classSymbol.Name}";
    string displayName =  className.EndsWith("Component") ? className.Replace("Component", "") : className;
    string baseUnityClass = isNetworkTickable ? "Unity.Netcode.NetworkBehaviour" : "MonoBehaviour";
    
    // Get all serialized fields
    IEnumerable<IFieldSymbol> editorFields = classSymbol.GetMembers()
      .OfType<IFieldSymbol>()
      .Where(_ => _.GetAttributes().Any(attribute => attribute.AttributeClass?.Name == nameof(EditorFieldAttribute)));
    
    // Check if the class has a constructor parameter with ParentAttribute
    bool hasParentAttributeParameter = classSymbol.Constructors
      .Any(constructor => constructor.Parameters.Any(param =>
        param.GetAttributes().Any(attr =>
          attr.AttributeClass?.Name == nameof(ParentAttribute))));

    // Construct all the strings needed for the serialized fields
    StringBuilder serializedFieldBuilder = new();
    StringBuilder serializedFieldChecksBuilder = new();
    StringBuilder serializedAssignmentsBuilder = new();
    foreach (IFieldSymbol editorField in editorFields)
    {
      if (editorField is null)
        continue;
      
      string fieldName = editorField.Name;
      string fieldType = editorField.Type.ToDisplayString().Replace("?", "");
      
      serializedFieldBuilder.AppendLine($"    [SerializeField] public {fieldType} {fieldName};");
      serializedAssignmentsBuilder.AppendLine($"      _component.{fieldName} = {fieldName};");
      
      if (editorField.NullableAnnotation == NullableAnnotation.Annotated)
        continue; 

      serializedFieldChecksBuilder.AppendLine($"      if ({fieldName} == null)");
      serializedFieldChecksBuilder.AppendLine($"        throw new Exception(\"{fieldName} cannot be null\");");
    }

    string callEnableFunction = classMethods.Any(m => m.Name == "OnEnabled") ? "OnEnable();" : "";
    sourceBuilder.Append($$"""
                           /*
                            * This class is automatically generated. 
                            * Any changes made to this file will be overwritten.
                           */
                            
                           #pragma warning disable   
                           #nullable enable
                           
                           using System;
                           using UnityEngine;
                                 
                           namespace ODK.Generated
                           {           
                             [AddComponentMenu("{{displayName}}")]
                             public class {{classSymbol.Name}}MonoBehaviour_generated : {{baseUnityClass}}
                             {
                           {{serializedFieldBuilder}}
                               private {{fullClassName}} _component;
                                 
                               public void Start() 
                               { 
                           {{serializedFieldChecksBuilder}}
                                 _component = {{classSymbol.Name}}DependencyResolver.CreateWithDependencies({{(hasParentAttributeParameter ? "this" : "")}});
                           {{serializedAssignmentsBuilder}}    
                                 {{callEnableFunction}}
                           }


                           """);

    if (classMethods.Any(m => m.Name == "OnEnabled"))
      sourceBuilder.Append("    public void OnEnable() { _component?.OnEnabled(); }\n");

    if (classMethods.Any(m => m.Name == "OnDisabled"))
      sourceBuilder.Append("    public void OnDisable() { _component?.OnDisabled(); }\n");

    if (classMethods.Any(m => m.Name == "OnTick" && TickMethodHasFloatParam(m.Parameters)))
      sourceBuilder.Append("    public void Update() { _component?.OnTick(Time.deltaTime); }\n");

    if (classMethods.Any(m => m.Name == "OnFixedTick"))
      sourceBuilder.Append("    public void FixedUpdate() { _component?.OnFixedTick(); }\n");

    sourceBuilder.Append("""
                             public void OnDestroy() 
                             {
                               if (_component is IDisposable disposable)
                                 disposable.Dispose();
                             }
                           }
                         }
                         #pragma warning restore
                         """);

    return sourceBuilder.ToString();
  }


  private bool TickMethodHasFloatParam(IList<IParameterSymbol> parameters)
  {
    return parameters.Count == 1 && parameters.Any(p => p.Type.SpecialType == SpecialType.System_Single);
  }
}