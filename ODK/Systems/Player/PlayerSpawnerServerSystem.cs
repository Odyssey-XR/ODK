#nullable enable

namespace OdysseyXR.ODK.Systems.Player
{
  using OdysseyXR.ODK.Commands.Player;
  using OdysseyXR.ODK.Components;
  using Unity.Collections;
  using Unity.Entities;
  using Unity.NetCode;
  using Unity.Transforms;
  using UnityEngine;

  /// <summary>
  /// Processes requests to spawn a player
  /// </summary>
  [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
  public partial struct PlayerSpawnServerSystem : ISystem
  {
    private ComponentLookup<NetworkId> _networkIdLookup;
    private Entity _playerPrefab;
    private DynamicBuffer<PlayerSpawnerTransformComponent> _playerSpawnerTransformsBuffer;
    private bool _lookedUpPlayerManager;

    public void OnCreate(ref SystemState state)
    {
      var entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp)
        .WithAll<SpawnPlayerClientRpc>()
        .WithAll<ReceiveRpcCommandRequest>();

      state.RequireForUpdate<PlayerSpawnerManagerComponent>();
      state.RequireForUpdate(state.GetEntityQuery(entityQueryBuilder));

      _networkIdLookup               = state.GetComponentLookup<NetworkId>(true);
    }

    public void OnUpdate(ref SystemState state)
    {
      _networkIdLookup.Update(ref state);

      if (!_lookedUpPlayerManager)
      {
        var playerSpawnerTransformsLookup = state.GetBufferLookup<PlayerSpawnerTransformComponent>(true);
        var playerSpawnerManager          = SystemAPI.GetSingleton<PlayerSpawnerManagerComponent>();
        _playerPrefab                     = playerSpawnerManager.PlayerPrefab;
        _playerSpawnerTransformsBuffer    = playerSpawnerTransformsLookup[playerSpawnerManager.Instance];

        _lookedUpPlayerManager = true;
      }
      
      var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
      foreach (var (rpcRequest, entity) in SystemAPI
        .Query<RefRO<ReceiveRpcCommandRequest>>()
        .WithAll<SpawnPlayerClientRpc>()
        .WithEntityAccess()
      )
      {
        var playerEntity = entityCommandBuffer.Instantiate(_playerPrefab);

        // Choose a random spawner location to start at
        var index               = UnityEngine.Random.Range(0, _playerSpawnerTransformsBuffer.Length);
        var playerSpawnerConfig = _playerSpawnerTransformsBuffer[index];

        entityCommandBuffer.SetComponent(playerEntity, new LocalTransform
        {
          Position = playerSpawnerConfig.Location,
          Rotation = Quaternion.Euler(playerSpawnerConfig.Rotation),
          Scale    = 1,
        });

        // Link the player prefab to the connection of the player
        entityCommandBuffer.SetComponent(playerEntity, new GhostOwner
        {
          NetworkId = _networkIdLookup[rpcRequest.ValueRO.SourceConnection].Value
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