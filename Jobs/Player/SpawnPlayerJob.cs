#nullable enable

namespace OdysseyXR.ODK.Jobs.Player
{
  using OdysseyXR.ODK.Commands.Player;
  using OdysseyXR.ODK.Components;
  using OdysseyXR.ODK.Extensions;
  using Unity.Burst;
  using Unity.Collections;
  using Unity.Entities;
  using Unity.NetCode;

  [BurstCompile]
  public partial struct SpawnPlayerJob : IJobEntity
  {
    public Unity.Mathematics.Random           RNG;
    public EntityCommandBuffer.ParallelWriter EntityCommandBuffer;

    [ReadOnly] public Entity                                         PlayerPrefab;
    [ReadOnly] public DynamicBuffer<PlayerSpawnerTransformComponent> SpawnerTransformsBuffer;
    [ReadOnly] public ComponentLookup<NetworkId>                     NetworkIdLookup;

    public void Execute(
      Entity sourceEntity,
      [EntityIndexInQuery] int sortKey,
      in ReceiveRpcCommandRequest rpcRequest,
      in SpawnPlayerClientRpc spawnPlayerRpc)
    {
      // Choose a random spawner location to start at
      var index               = RNG.NextInt(0, SpawnerTransformsBuffer.Length);
      var playerSpawnerConfig = SpawnerTransformsBuffer[index];

      // Spawn the player entity
      var playerEntity = EntityCommandBuffer.SpawnEntity(
        sortKey, 
        PlayerPrefab, 
        playerSpawnerConfig.Location,
        playerSpawnerConfig.Rotation,
        1
      );

      // Link the player prefab to the connection of the player
      EntityCommandBuffer.SetComponent(sortKey, playerEntity, new GhostOwner
      {
        NetworkId = NetworkIdLookup[rpcRequest.SourceConnection].Value
      });
      EntityCommandBuffer.AppendToBuffer(sortKey, rpcRequest.SourceConnection, new LinkedEntityGroup
      {
        Value = playerEntity
      });
      EntityCommandBuffer.DestroyEntity(sortKey, sourceEntity);
    }
  }
}