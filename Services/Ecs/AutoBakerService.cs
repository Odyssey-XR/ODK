#if UNITY_EDITOR
#nullable enable

namespace Unity.Entities
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Reflection;
  using System.Text;
  using OdysseyXR.ODK.Attributes.ECS;
  using OdysseyXR.ODK.Behaviours.ECS;
  using Unity.VisualScripting.FullSerializer.Internal;
  using UnityEditor;
  using UnityEngine;
  using Logger = OdysseyXR.ODK.Services.Logging.Logger;

  [InitializeOnLoad]
  public class AutoBakerService
  {
    private struct BakedPropertyData
    {
      public string    BakedName;
      public FieldInfo AuthoredTypeInfo;
      public bool      IsManagedType;
    }

    static AutoBakerService()
    {
      EditorApplication.delayCall += Update;
      CheckForAutoBakerRequests();
    }

    static void Update()
    {
      if (EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
        CheckForAutoBakerRequests();
    }

    static void CheckForAutoBakerRequests()
    {
      TypeCache.GetTypesDerivedFrom<MonoBehaviour>()
        .Where(t => t.GetCustomAttribute<AutoBakerAttribute>() is not null)
        .ToList()
        .ForEach(CreateAutoBakerForType);
    }

    static void CreateAutoBakerForType(Type authoredType)
    {
      var autoBakerAttribute = authoredType.GetCustomAttribute<AutoBakerAttribute>();

      // Get all fields that need to be baked along with all entity references
      var bakingFields = authoredType
        .GetDeclaredFields()
        .Where(field => field.GetCustomAttribute<BakeAsFieldAttribute>() is not null)
        .Select(field => new BakedPropertyData
        {
          BakedName        = field.GetCustomAttribute<BakeAsFieldAttribute>().Name ?? field.Name,
          AuthoredTypeInfo = field,
          IsManagedType    = !field.FieldType.IsValueType,
        });

      var bakingEntities = authoredType
        .GetDeclaredFields()
        .Where(field => field.GetCustomAttribute<BakeAsEntityAttribute>() is not null)
        .Select(field => new BakedPropertyData
        {
          BakedName        = field.GetCustomAttribute<BakeAsEntityAttribute>().Name ?? field.Name,
          AuthoredTypeInfo = field,
          IsManagedType    = true,
        });

      var namespaces = bakingFields
        .Concat(bakingEntities)
        .ToList()
        .Select(field => field.AuthoredTypeInfo.FieldType.Namespace)
        .Concat(new List<string> { authoredType.Namespace });

      if (autoBakerAttribute.ComponentType is not null)
        namespaces = namespaces.Concat(new List<string> { autoBakerAttribute.ComponentType.Namespace });
      namespaces = namespaces.Distinct();

      var files = new List<(string, string)>();

      // If no component was specified on the attribute then we also want to create
      // a new component
      string componentName;
      if (autoBakerAttribute.ComponentType is null)
      {
        files.Add((GenerateComponentCode(authoredType, bakingFields, bakingEntities, namespaces), "Component"));
        componentName = $"{authoredType.Name}Component";
      }
      else
      {
        componentName = autoBakerAttribute.ComponentType.Name;
      }

      files.Add((
        GenerateBakerCode(authoredType, componentName, bakingFields, bakingEntities, namespaces),
        "Baker"));
      foreach (var file in files)
        GenerateFile(authoredType, file.Item1, file.Item2);
    }

    static string GenerateComponentCode(
      Type authoredType,
      IEnumerable<BakedPropertyData> bakingFields,
      IEnumerable<BakedPropertyData> entityFields,
      IEnumerable<string> namespaces)
    {
      var sb                 = new StringBuilder();
      var componentName      = $"{authoredType.Name}Component";
      var isManagedComponent = bakingFields.Any(field => field.IsManagedType);
      var componentDataType  = isManagedComponent ? "class" : "struct";

      sb.AppendLine("using Unity.Entities;");
      foreach (var assembly in namespaces)
        sb.AppendLine($"using {assembly};");

      sb.AppendLine();
      sb.AppendLine($"public partial {componentDataType} {componentName} : IComponentData");
      sb.AppendLine("{");

      foreach (var field in bakingFields)
        sb.AppendLine($"  public {field.AuthoredTypeInfo.FieldType} {field.BakedName};");

      foreach (var field in entityFields)
        sb.AppendLine($"  public Entity {field.BakedName};");

      sb.AppendLine("}");

      return sb.ToString();
    }

    static string GenerateBakerCode(
      Type authoredType,
      string componentName,
      IEnumerable<BakedPropertyData> bakingFields,
      IEnumerable<BakedPropertyData> bakingEntities,
      IEnumerable<string> namespaces)
    {
      var sb        = new StringBuilder();
      var bakerName = $"{authoredType.Name}Baker";

      sb.AppendLine("using System.Collections.Generic;");
      sb.AppendLine("using Unity.Entities;");
      foreach (var assembly in namespaces)
        sb.AppendLine($"using {assembly};");
      sb.AppendLine("using OdysseyXR.ODK.Attributes.ECS;");
      sb.AppendLine("using OdysseyXR.ODK.Components.ECS;");

      sb.AppendLine();
      sb.AppendLine($"public partial class {bakerName} : Baker<{authoredType.Name}>");
      sb.AppendLine($"{{");
      sb.AppendLine($"  public override void Bake({authoredType.Name} authoring)");
      sb.AppendLine($"  {{");
      sb.AppendLine($"    var entity = GetEntity(TransformUsageFlags.None);");
      sb.AppendLine($"    var bakedComponent = new {componentName}() {{");

      foreach (var field in bakingFields)
        sb.AppendLine($"      {field.BakedName} = authoring.{field.AuthoredTypeInfo.Name},");

      sb.AppendLine($"    }};");
      sb.AppendLine();
      sb.AppendLine($"    AddComponent(entity, bakedComponent);");
      sb.AppendLine();

      foreach (var entity in bakingEntities)
      {
        sb.AppendLine($"    var {entity.BakedName}Entity = CreateAdditionalEntity(TransformUsageFlags.None);");
        sb.AppendLine($"    AddComponent<PatchEntityReferenceRequestComponent>({entity.BakedName}Entity);");
        sb.AppendLine($"    AddComponentObject({entity.BakedName}Entity, new PatchEntityReferenceDataComponent() {{");
        sb.AppendLine($"      AuthoredBehaviour = (AuthoredBehaviour)authoring.{entity.AuthoredTypeInfo.Name},");
        sb.AppendLine($"      ComponentData = &bakedComponent,");
        sb.AppendLine($"      BakedName = \"{entity.BakedName}\",");
        sb.AppendLine($"    }});");
        sb.AppendLine();
      }

      if (authoredType.IsSubclassOf(typeof(AuthoredBehaviour)))
        sb.AppendLine("    authoring.AuthoredEntity = entity;");
      sb.AppendLine($"  }}");
      sb.AppendLine($"}}");

      return sb.ToString();
    }

    static void GenerateFile(Type authoredType, string code, string type)
    {
      var typeName  = $"{authoredType.Name}{type}.cs";
      var directory = Path.Combine(Application.dataPath, "Generated", "ODK", $"{type}s");

      if (!Directory.Exists(directory))
        Directory.CreateDirectory(directory);

      var filePath = Path.Combine(directory, typeName);

      File.WriteAllText(filePath, code);
      AssetDatabase.Refresh();
    }
  }
}
#endif