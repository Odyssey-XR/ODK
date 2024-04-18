using OdysseyXR.ODK.Behaviours.ECS;

namespace OdysseyXR.ODK.Components.ECS
{
  using System.Collections.Generic;
  using Unity.Entities;

  public class PatchEntityReferenceDataComponent : IComponentData
  {
    public Entity                  SourceEntity;
    public IComponentData          SourceComponent;
    public List<AuthoredBehaviour> ReferencedBehaviours;
    public List<string>            BakedEntityNames;
  }
}