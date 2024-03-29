#nullable enable

namespace OdysseyXR.ODK.Extensions
{
  using Unity.Entities;
  using Unity.Mathematics;
  using Unity.NetCode;
  using Unity.Transforms;

  /// <summary>
  /// Extensions methods for the <see cref="EntityCommandBuffer"/>
  /// </summary>
  public static class EntityCommandBufferExtensions
  {
    /// <summary>
    /// Adds a <see cref="SendRpcCommandRequest"/> to an entity
    /// </summary>
    /// <param name="entityCommandBuffer">
    /// The <see cref="EntityCommandBuffer"/>
    /// </param>
    /// <param name="entity">
    /// The <see cref="Entity"/> to add the <see cref="SendRpcCommandRequest"/> onto
    /// </param>
    /// <param name="targetConnection">
    /// The target connection for the <see cref="SendRpcCommandRequest"/>
    /// </param>
    /// <typeparam name="T">
    /// The rpc command data to be sent along with the request
    /// </typeparam>
    public static void AddRpcRequest<T>(
      this EntityCommandBuffer entityCommandBuffer,
      Entity entity,
      Entity targetConnection
    ) where T : unmanaged, IRpcCommand
    {
      entityCommandBuffer.AddComponent<T>(entity);
      entityCommandBuffer.AddComponent(entity, new SendRpcCommandRequest
      {
        TargetConnection = targetConnection
      });
    }

    /// <summary>
    /// Adds a <see cref="SendRpcCommandRequest"/> to an entity
    /// </summary>
    /// <param name="entityCommandBuffer">
    /// The <see cref="EntityCommandBuffer"/>
    /// </param>
    /// <param name="entity">
    /// The <see cref="Entity"/> to add the <see cref="SendRpcCommandRequest"/> onto
    /// </param>
    /// <param name="rpcRequest">
    /// The rpc command data to be sent along with the request
    /// </param>
    /// <param name="targetConnection">
    /// The target connection for the <see cref="SendRpcCommandRequest"/>
    /// </param>
    /// <typeparam name="T">
    /// The rpc command data to be sent along with the request
    /// </typeparam>
    public static void AddRpcRequest<T>(
      this EntityCommandBuffer entityCommandBuffer,
      Entity entity,
      T rpcRequest,
      Entity targetConnection
    ) where T : unmanaged, IRpcCommand
    {
      entityCommandBuffer.AddComponent(entity, rpcRequest);
      entityCommandBuffer.AddComponent(entity, new SendRpcCommandRequest
      {
        TargetConnection = targetConnection
      });
    }

    /// <summary>
    /// Adds a <see cref="SendRpcCommandRequest"/> to a new entity
    /// </summary>
    /// <param name="entityCommandBuffer">
    /// The <see cref="EntityCommandBuffer"/>
    /// </param>
    /// <typeparam name="T">
    /// The rpc command data to be sent along with the request
    /// </typeparam>
    public static void SendRpcRequest<T>(
      this EntityCommandBuffer entityCommandBuffer,
      Entity targetConnection
    ) where T : unmanaged, IRpcCommand
    {
      var entity = entityCommandBuffer.CreateEntity();
      entityCommandBuffer.AddComponent<T>(entity);
      entityCommandBuffer.AddComponent(entity, new SendRpcCommandRequest
      {
        TargetConnection = targetConnection
      });
    }

    /// <summary>
    /// Adds a <see cref="SendRpcCommandRequest"/> to a new entity
    /// </summary>
    /// <param name="entityCommandBuffer">
    /// The <see cref="EntityCommandBuffer"/>
    /// </param>
    /// <param name="targetConnection">
    /// The target connection for the <see cref="SendRpcCommandRequest"/>
    /// </param>
    /// <param name="rpcRequest">
    /// The rpc command data to be sent along with the request
    /// </param>
    /// <typeparam name="T">
    /// The rpc command data to be sent along with the request
    /// </typeparam>
    public static void SendRpcRequest<T>(
      this EntityCommandBuffer entityCommandBuffer,
      T rpcRequest,
      Entity targetConnection
    ) where T : unmanaged, IRpcCommand
    {
      var entity = entityCommandBuffer.CreateEntity();
      entityCommandBuffer.AddComponent(entity, rpcRequest);
      entityCommandBuffer.AddComponent(entity, new SendRpcCommandRequest
      {
        TargetConnection = targetConnection
      });
    }

    /// <summary>
    /// Spawns a new entity
    /// </summary>
    /// <param name="entityCommandBuffer">
    /// The <see cref="EntityCommandBuffer"/>
    /// </param>
    /// <param name="entityPrefab">
    /// The <see cref="Entity"/> to spawn
    /// </param>
    /// <param name="location">
    /// The starting location of the spawned entity
    /// </param>
    /// <param name="rotation">
    /// The starting rotation of the spawned entity
    /// </param>
    /// <param name="scale">
    /// The starting scale of the spawned entity
    /// </param>
    /// <returns>
    /// The spawned entity
    /// </returns>
    public static Entity SpawnEntity(
      this EntityCommandBuffer entityCommandBuffer,
      Entity entityPrefab,
      float3 location,
      float3 rotation,
      int scale)
    {
      var playerEntity = entityCommandBuffer.Instantiate(entityPrefab);
      entityCommandBuffer.SetComponent(playerEntity, new LocalTransform
      {
        Position = location,
        Rotation = quaternion.Euler(rotation),
        Scale    = scale,
      });
      return playerEntity;
    }

    /// <summary>
    /// Spawns a new entity
    /// </summary>
    /// <param name="entityCommandBuffer">
    /// The <see cref="EntityCommandBuffer.ParallelWriter"/>
    /// </param>
    /// <param name="sortKey">
    /// The index of the entity being operated on in a parallel systemAPI query
    /// </param>
    /// <param name="entityPrefab">
    /// The <see cref="Entity"/> to spawn
    /// </param>
    /// <param name="location">
    /// The starting location of the spawned entity
    /// </param>
    /// <param name="rotation">
    /// The starting rotation of the spawned entity
    /// </param>
    /// <param name="scale">
    /// The starting scale of the spawned entity
    /// </param>
    /// <returns>
    /// The spawned entity
    /// </returns>
    public static Entity SpawnEntity(
      this EntityCommandBuffer.ParallelWriter entityCommandBuffer,
      int sortKey,
      Entity entityPrefab,
      float3 location,
      float3 rotation,
      int scale)
    {
      var playerEntity = entityCommandBuffer.Instantiate(sortKey, entityPrefab);
      entityCommandBuffer.SetComponent(sortKey, playerEntity, new LocalTransform
      {
        Position = location,
        Rotation = quaternion.Euler(rotation),
        Scale    = scale,
      });
      return playerEntity;
    }
  }
}