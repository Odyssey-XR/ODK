#nullable enable

using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace OdysseyXR.ODK.Systems.Player
{
  using OdysseyXR.ODK.Commands.Player;
  using OdysseyXR.ODK.Components;
  using OdysseyXR.ODK.Core.Logging;
  using Unity.Collections;
  using Unity.Entities;
  using Unity.NetCode;

  /// <summary>
  /// Processes requests to spawn a player
  /// </summary>
  [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
  public partial struct PlayerSpawnServerSystem : ISystem
  {
    public ComponentLookup<PlayerPrefabComponent> PlayerPrefabComponentLookup;
    public BufferLookup<PlayerSpawnerComponent>   PlayerSpawnerComponentLookup;
    public ComponentLookup<NetworkId>             NetworkIdComponentLookup;

    public void OnCreate(ref SystemState state)
    {
      var entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp)
        .WithAll<SpawnPlayerClientRpc>()
        .WithAll<ReceiveRpcCommandRequest>();

      state.RequireForUpdate<PlayerSpawnerInstanceComponent>();
      state.RequireForUpdate(state.GetEntityQuery(entityQueryBuilder));

      PlayerPrefabComponentLookup  = state.GetComponentLookup<PlayerPrefabComponent>(true);
      PlayerSpawnerComponentLookup = state.GetBufferLookup<PlayerSpawnerComponent>(true);
      NetworkIdComponentLookup     = state.GetComponentLookup<NetworkId>(true);
    }

    public void OnUpdate(ref SystemState state)
    {
      PlayerPrefabComponentLookup.Update(ref state);
      PlayerSpawnerComponentLookup.Update(ref state);
      NetworkIdComponentLookup.Update(ref state);
      
      var playerSpawnerManagerEntity      = SystemAPI.GetSingleton<PlayerSpawnerInstanceComponent>().Instance;
      if (!PlayerSpawnerComponentLookup.HasBuffer(playerSpawnerManagerEntity))
      {
        Logger.Error("Player spawner manager does not have an initialized spawner config buffer");
        return;
      }

      var playerSpawnerConfigBuffer = PlayerSpawnerComponentLookup[playerSpawnerManagerEntity];
      if (playerSpawnerConfigBuffer.Length <= 0)
      {
        Logger.Error("Player spawner manager spawner config buffer has no entries");
        return;
      }

      var playerPrefab        = PlayerPrefabComponentLookup[playerSpawnerManagerEntity].PlayerPrefab;
      var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

      foreach (var (rpcRequest, spawnPlayerClientRpc, entity) in SystemAPI
                 .Query<RefRO<ReceiveRpcCommandRequest>, RefRO<SpawnPlayerClientRpc>>()
                 .WithEntityAccess()
              )
      {
        var playerEntity = entityCommandBuffer.Instantiate(playerPrefab);

        // Choose a random spawner location to start at
        var index               = UnityEngine.Random.Range(0, playerSpawnerConfigBuffer.Length);
        var playerSpawnerConfig = playerSpawnerConfigBuffer[index];

        entityCommandBuffer.SetComponent(playerEntity, new LocalTransform
        {
          Position = playerSpawnerConfig.Location,
          Rotation = Quaternion.Euler(playerSpawnerConfig.Rotation),
          Scale    = 1,
        });

        // Link the player prefab to the connection of the player
        entityCommandBuffer.SetComponent(playerEntity, new GhostOwner
        {
          NetworkId = NetworkIdComponentLookup[rpcRequest.ValueRO.SourceConnection].Value
        });
        entityCommandBuffer.AppendToBuffer(rpcRequest.ValueRO.SourceConnection, new LinkedEntityGroup
        {
          Value = playerEntity
        });
        entityCommandBuffer.DestroyEntity(entity);
      }

      entityCommandBuffer.Playback(state.EntityManager);
    }
  }
}