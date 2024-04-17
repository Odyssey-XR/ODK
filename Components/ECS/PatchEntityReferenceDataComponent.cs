namespace OdysseyXR.ODK.Components.ECS
{
  using System;
  using OdysseyXR.ODK.Behaviours.ECS;
  using Unity.Entities;

  public unsafe class PatchEntityReferenceDataComponent : IComponentData
  {
    public AuthoredBehaviour AuthoredBehaviour;
    public Type              ComponentType;
    public string            BakedName;
  }
}