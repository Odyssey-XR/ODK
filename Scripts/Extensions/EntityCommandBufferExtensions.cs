#nullable enable

namespace OdysseyXR.ODK.Extensions
{
  using Unity.Entities;
  using Unity.NetCode;

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
  }
}