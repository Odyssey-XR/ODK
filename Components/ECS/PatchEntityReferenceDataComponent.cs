using OdysseyXR.ODK.Behaviours.ECS;

namespace OdysseyXR.ODK.Components.ECS
{
  using Unity.Entities;

  public class PatchEntityReferenceDataComponent : IComponentData 
  {
    public Entity SourceEntity;
    public AuthoredBehaviour ReferencedBehaviour;
    public string BakedEntityName;
  }
}