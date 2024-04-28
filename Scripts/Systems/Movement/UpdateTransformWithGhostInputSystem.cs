#nullable enable

namespace OdysseyXR.ODK.Systems.Movement
{
  using OdysseyXR.ODK.Components.Movement;
  using Unity.Burst;
  using Unity.Collections;
  using Unity.Entities;
  using Unity.NetCode;
  using Unity.Transforms;
  using UnityEngine;

  [BurstCompile]
  [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
  public partial struct UpdateTransformWithGhostInputSystem : ISystem
  {
    public void OnCreate(ref SystemState state)
    {
      var entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp)
        .WithAll<LocalTransform, GhostTransformInputComponent>();

      state.RequireForUpdate(state.GetEntityQuery(entityQueryBuilder));
    }

    public void OnUpdate(ref SystemState state)
    {
      foreach (var (sourceTransform, trackedTransform) in SystemAPI
                 .Query<RefRW<LocalTransform>, RefRO<GhostTransformInputComponent>>()
                 .WithAll<Simulate>())
      {
        sourceTransform.ValueRW.Position = trackedTransform.ValueRO.Position;
        sourceTransform.ValueRW.Rotation = Quaternion.Euler(trackedTransform.ValueRO.Rotation);
      }
    }
  }
}