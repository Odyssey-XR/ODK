#nullable enable

namespace OdysseyXR.ODK.Systems.Player
{
  using OdysseyXR.ODK.Commands.Player;
  using OdysseyXR.ODK.Components;
  using OdysseyXR.ODK.Jobs.Player;
  using Unity.Burst;
  using Unity.Collections;
  using Unity.Entities;
  using Unity.NetCode;

  /// <summary>
  /// Processes requests to spawn a player
  /// </summary>
  [BurstCompile]
  [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
  public partial struct PlayerSpawnServerSystem : ISystem, ISystemStartStop
  {
    private Unity.Mathematics.Random                       _rng;
    private ComponentLookup<NetworkId>                     _networkIdLookup;
    private PlayerSpawnerManagerComponent                  _playerSpawnerManager;
    private Entity                                         _playerPrefab;
    private DynamicBuffer<PlayerSpawnerTransformComponent> _playerSpawnerTransformsBuffer;

    public void OnCreate(ref SystemState state)
    {
      // TODO: We want to do something better than getting a random number based on a constant seed
      // since this is reset everytime `OnCreate` is called.
      // A better solution would be to loop through all spawners and select the first one which isn't currently
      // occupied (i.e a player is not being spawned and a player is not standing on the spawner)
      _rng = new Unity.Mathematics.Random(1);

      var entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp)
        .WithAll<ReceiveRpcCommandRequest>()
        .WithAll<SpawnPlayerClientRpc>();

      state.RequireForUpdate<PlayerSpawnerManagerComponent>();
      state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
      state.RequireForUpdate(state.GetEntityQuery(entityQueryBuilder));

      _networkIdLookup = state.GetComponentLookup<NetworkId>(true);
    }

    public void OnStartRunning(ref SystemState state)
    {
      var playerSpawnerTransformsLookup = state.GetBufferLookup<PlayerSpawnerTransformComponent>(true);

      _playerSpawnerManager          = SystemAPI.GetSingleton<PlayerSpawnerManagerComponent>();
      _playerPrefab                  = _playerSpawnerManager.PlayerPrefab;
      _playerSpawnerTransformsBuffer = playerSpawnerTransformsLookup[_playerSpawnerManager.Instance];
    }

    public void OnStopRunning(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
      _networkIdLookup.Update(ref state);

      var ecbSystemSingleton  = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
      var entityCommandBuffer = ecbSystemSingleton.CreateCommandBuffer(state.WorldUnmanaged);
      new SpawnPlayerJob
      {
        RNG                     = _rng,
        EntityCommandBuffer     = entityCommandBuffer.AsParallelWriter(),
        PlayerPrefab            = _playerPrefab,
        SpawnerTransformsBuffer = _playerSpawnerTransformsBuffer,
        NetworkIdLookup         = _networkIdLookup,
      }.ScheduleParallel();
    }
  }
}