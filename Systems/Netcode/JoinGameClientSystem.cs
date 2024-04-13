#nullable enable

namespace OdysseyXR.ODK.Systems.NetCode
{
  using OdysseyXR.ODK.Commands.NetCode;
  using OdysseyXR.ODK.Extensions;
  using Unity.Burst;
  using Unity.Collections;
  using Unity.Entities;
  using Unity.NetCode;

  /// <summary>
  /// Process new connection requests to join the game
  /// </summary>
  [BurstCompile]
  [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
  public partial struct JoinGameClientSystem : ISystem
  {
    public void OnCreate(ref SystemState state)
    {
      var entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp)
        .WithAll<NetworkId>()
        .WithNone<NetworkStreamInGame>();

      state.RequireForUpdate(state.GetEntityQuery(entityQueryBuilder));
    }

    public void OnUpdate(ref SystemState state)
    {
      using var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
      foreach (var (_, entity) in SystemAPI
        .Query<RefRO<NetworkId>>()
        .WithNone<NetworkStreamInGame>()
        .WithEntityAccess()
      )
      {
        entityCommandBuffer.AddComponent<NetworkStreamInGame>(entity);
        entityCommandBuffer.SendRpcRequest<JoinGameClientRpc>(entity);
      }

      entityCommandBuffer.Playback(state.EntityManager);
    }
  }
}
