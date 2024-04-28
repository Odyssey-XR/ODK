#nullable enable

namespace OdysseyXR.ODK.Components.ECS
{
  using System.Collections.Generic;
  using OdysseyXR.ODK.Behaviours.ECS;
  using Unity.Entities;

  public class PatchEntityReferenceDataComponent : IComponentData
  {
    public Entity                  SourceEntity;
    public IComponentData          SourceComponent = null!;
    public List<AuthoredBehaviour> ReferencedBehaviours = null!;
    public List<string>            BakedEntityNames = null!;
  }
}