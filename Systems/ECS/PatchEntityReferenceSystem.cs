#if UNITY_EDITOR
namespace Plugins.ODK.Systems.ECS
{
  using OdysseyXR.ODK.Behaviours.ECS;
  using OdysseyXR.ODK.Components.ECS;
  using OdysseyXR.ODK.Services.Logging;
  using Unity.Collections;
  using Unity.Entities;
  using Unity.VisualScripting;

  [WorldSystemFilter(WorldSystemFilterFlags.BakingSystem)]
  public unsafe partial struct PatchEntityReferenceSystem : ISystem
  {
    public void OnUpdate(ref SystemState state)
    {
      var ecb = new EntityCommandBuffer(Allocator.Temp);
      foreach (var (patchEntityRequest, entity) in SystemAPI.Query<RefRO<PatchEntityReferenceRequestComponent>>()
                 .WithEntityAccess())
      {
        if (state.EntityManager.HasComponent<PatchEntityReferenceDataComponent>(entity))
        {
          var patchEntityData = state.EntityManager.GetComponentObject<PatchEntityReferenceDataComponent>(entity);

          if (!patchEntityData.AuthoredBehaviour.GetType().IsSubclassOf(typeof(AuthoredBehaviour)))
          {
            Logger.Error(
              $"Baked entity {patchEntityData.AuthoredBehaviour.GetType().Name} must be derived from type AuthoredBehaviour");
            continue;
          }

          var componentType = state.EntityManager.GetComponent(entity);
          var entityField   = componentType.GetField(patchEntityData.BakedName);
          entityField.SetValue(*patchEntityData.ComponentData, patchEntityData.AuthoredBehaviour.AuthoredEntity);
        }

        // ecb.DestroyEntity(entity);
      }

      ecb.Playback(state.EntityManager);
    }
  }
}
#endif