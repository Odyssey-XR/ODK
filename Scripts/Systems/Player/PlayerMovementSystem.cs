#nullable enable

namespace Plugins.ODK.Scripts.Systems.Player
{
  using OdysseyXR.ODK.Components.Player;
  using OdysseyXR.ODK.Services.Logging;
  using Plugins.ODK.Components.XR;
  using Unity.Collections;
  using Unity.Entities;
  using Unity.Mathematics;
  using Unity.NetCode;
  using Unity.Transforms;

  [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
  public partial struct PlayerMovementSystem : ISystem
  {
    public void OnCreate(ref SystemState state)
    {
      var entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp)
        .WithAll<LocalTransform, GhostControllerActionInputComponent, PlayerTag>();

      state.RequireForUpdate(state.GetEntityQuery(entityQueryBuilder));
    }

    public void OnUpdate(ref SystemState state)
    {
      var speed = SystemAPI.Time.DeltaTime * 2;
      foreach (var (playerTransform, playerInput) in SystemAPI
                 .Query<RefRW<LocalTransform>, RefRO<GhostControllerActionInputComponent>>()
                 .WithAll<PlayerTag, Simulate>())
      {
        var direction = new float3(playerInput.ValueRO.Thumbstick.x, 0, playerInput.ValueRO.Thumbstick.y);
        var movement  = math.normalizesafe(direction) * speed;

        Logger.Log(movement.ToString());
        Logger.Log();
        
        playerTransform.ValueRW.Position += movement;
      }
    }
  }
}