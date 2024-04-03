namespace OdysseyXR.Arcadia.Plugins.ODK.Core.Ecs
{
  using System.Reflection;
  using Unity.Entities;

#if UNITY_EDITOR
  public class LookupEntityDataComponent : IComponentData 
  {
    public FieldInfo         propertyInfo;
    public IComponentData    propertyComponent;
    public AuthoredBehaviour SourceEntity;
  }
#endif
}