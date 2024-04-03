#nullable enable

namespace OdysseyXR.Arcadia.Plugins.ODK.Core.Ecs
{
  using System.Reflection;
  using Unity.Entities;
  using Unity.VisualScripting.FullSerializer.Internal;
  using UnityEngine;
  using Logger = OdysseyXR.ODK.Core.Logging.Logger;

  public abstract class EntityBaker<AuthoringBehaviour, ComponentData> : Baker<AuthoringBehaviour>
  where AuthoringBehaviour : AuthoredBehaviour
  where ComponentData : unmanaged, IComponentData
  {
    private readonly TransformUsageFlags _transformUsageFlags;

    protected EntityBaker(TransformUsageFlags transformUsageFlags = TransformUsageFlags.None)
    {
      _transformUsageFlags = transformUsageFlags;
    }

    public override void Bake(AuthoringBehaviour authoring)
    {
      foreach (var property in authoring.GetType().GetDeclaredFields())
      {
        if (property.GetValue(authoring) is Object dependency)
          DependsOn(dependency);
      }

      var componentDataType     = typeof(ComponentData);
      var authoredBehaviourType = typeof(AuthoringBehaviour);

      // Create the component and populate it's fields
      var component = default(ComponentData);
      foreach (var componentPropery in componentDataType.GetFields())
      {
        var propertyPath          = componentPropery.Name;
        var authoredNameAttribute = componentPropery.GetCustomAttribute<AuthoredNameAttribute>();
        var lookupEntityAttribute = componentPropery.GetCustomAttribute<LookupEntityAttribute>();
        if (lookupEntityAttribute is not null)
          propertyPath = lookupEntityAttribute.PropertyPath;
        else if (authoredNameAttribute is not null)
          propertyPath = authoredNameAttribute.PropertyPath;

        var authoredBehaviourProperty = GetPropertyValue(authoring, propertyPath);
        if (lookupEntityAttribute is not null)
        {
          var lookupEntity = CreateAdditionalEntity(TransformUsageFlags.None);
          AddComponent<LookupEntityComponent>(lookupEntity);
          AddComponentObject(lookupEntity, new LookupEntityDataComponent
            {
              propertyInfo = componentPropery,
              propertyComponent = component,
              SourceEntity = authoredBehaviourProperty as AuthoredBehaviour
            }
          );
          
          // We can continue here as the LookupEntityBakerSystem will patch the values later
          // after all entities have finished baking
          continue;
        }

        if (authoredBehaviourProperty is null && authoredNameAttribute is not null && !authoredNameAttribute.AllowNull)
        {
          Logger.Warn(
            $"Component cannot find property `{propertyPath}` on `{authoredBehaviourType}` or the value is null"
          );
          continue;
        }

        componentPropery.SetValue(
          component,
          authoredBehaviourProperty
        );
      }

      // Create the entity from the behaviour
      var entity = GetEntity(_transformUsageFlags);
      AddComponent(entity, component);

      authoring.AuthoredEntity = entity;
    }

    private object? GetPropertyValue(object? src, string propertyPath)
    {
      // Complex nested path
      if (propertyPath.Contains("."))
      {
        var paths = propertyPath.Split(new[] { '.' }, 2);
        return GetPropertyValue(GetPropertyValue(src, paths[0]), paths[1]);
      }

      var property = src?.GetType().GetProperty(propertyPath);
      return property?.GetValue(src);
    }
  }
}