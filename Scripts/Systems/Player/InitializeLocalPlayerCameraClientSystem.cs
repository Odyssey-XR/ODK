#nullable enable

namespace Plugins.ODK.Scripts.Systems.Player
{
  using OdysseyXR.ODK.Components.Player;
  using Plugins.ODK.Behaviours.XR;
  using Unity.Collections;
  using Unity.Entities;
  using UnityEngine;
  using Logger = OdysseyXR.ODK.Services.Logging.Logger;

  /// <summary>
  /// When a local player joins the client world for the first time,
  /// we want to check if there are any cameras in the scene that should be attached to the player.
  /// If there are, then we add a <see cref="XRCamera"/> component to the camera, and set it's
  /// target to the <see cref="XRPlayerComponent"/>'s HMD entity.
  /// </summary>
  [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
  public partial struct InitializeLocalPlayerCameraClientSystem : ISystem
  {
    public void OnCreate(ref SystemState state)
    {
      var entityQueryBuilder = new EntityQueryBuilder(Allocator.Temp)
        .WithAll<LocallyOwnedPlayer, XRPlayerComponent>();

      state.RequireForUpdate(state.GetEntityQuery(entityQueryBuilder));
    }

    public void OnUpdate(ref SystemState state)
    {
      state.Enabled = false;

      var mainCamera = Camera.main;
      var xrCamera   = mainCamera?.GetComponent<XRCamera>();
      foreach (var xrPlayer in SystemAPI.Query<RefRO<XRPlayerComponent>>().WithAll<LocallyOwnedPlayer>())
      {
        if (mainCamera is null)
          return;

        xrCamera ??= mainCamera.gameObject.AddComponent<XRCamera>();

        xrCamera.HMDEntity         = xrPlayer.ValueRO.HMDTracker;
        xrCamera.FloorOriginEntity = xrPlayer.ValueRO.FloorOrigin;
        Logger.Log("Finished setting up XRCamera for local player");
      }
    }
  }
}