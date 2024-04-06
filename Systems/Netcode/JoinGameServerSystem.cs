#nullable enable

namespace OdysseyXR.ODK.Systems.NetCode
{
  using OdysseyXR.ODK.Commands.NetCode;
  using Unity.Burst;
  using Unity.Collections;
  using Unity.Entities;
  using Unity.NetCode;

  /// <summary>
  /// Process requests to join the game
  /// </summary>
  [BurstCompile]
  [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
  public partial struct JoinGameServerSystem : ISystem
  {
    private ComponentLookup<NetworkId> _networkIdComponentLookup;

    public void OnCreate(ref SystemState state)
    {
      var entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp)
        .WithAll<JoinGameClientRpc>()
        .WithAll<ReceiveRpcCommandRequest>();

      state.RequireForUpdate(state.GetEntityQuery(entityQueryBuilder));

      _networkIdComponentLookup = state.GetComponentLookup<NetworkId>(true);
    }

    public void OnUpdate(ref SystemState state)
    {
      var worldName = state.WorldUnmanaged.Name;
      var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

      _networkIdComponentLookup.Update(ref state);

      foreach (var (receivedRpc, entity) in SystemAPI
        .Query<RefRO<ReceiveRpcCommandRequest>>()
        .WithAll<JoinGameClientRpc>()
        .WithEntityAccess()
      )
      {
        entityCommandBuffer.AddComponent<NetworkStreamInGame>(
          receivedRpc.ValueRO.SourceConnection
        );

        var networkId = _networkIdComponentLookup[receivedRpc.ValueRO.SourceConnection];
        Core.Logging.Logger.Log($"`{worldName}` setting connection `{networkId.Value}` to in game");

        entityCommandBuffer.DestroyEntity(entity);
      }

      entityCommandBuffer.Playback(state.EntityManager);
    }
  }
}
