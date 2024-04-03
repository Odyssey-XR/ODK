namespace OdysseyXR.Arcadia.Plugins.ODK.Core.Ecs
{
  using Unity.Entities;

  [WorldSystemFilter(WorldSystemFilterFlags.BakingSystem)]
  public partial class LookupEntityBakerSystem : SystemBase
  {
    protected override void OnUpdate()
    {
      Entities
        .WithStructuralChanges()
        .WithAll<LookupEntityComponent>()
        .ForEach((Entity entity) =>
          {
            var lookupEntityDataComponent = EntityManager.GetComponentObject<LookupEntityDataComponent>(entity);
            lookupEntityDataComponent.propertyInfo.SetValue(
              lookupEntityDataComponent.propertyComponent,
              lookupEntityDataComponent.SourceEntity.AuthoredEntity
            );
            EntityManager.DestroyEntity(entity);
          }
        )
        .Run();
    }
  }
}