#nullable enable

namespace OdysseyXR.ODK.Behaviours
{
  using System;
  using UnityEngine;
  using Unity.Entities;
  using OdysseyXR.ODK.Components;
  using Logger = OdysseyXR.ODK.Core.Logging.Logger;

  /// <summary>
  /// Manager class for player spawners. Holds references to potential spawn locations for a player
  /// </summary>
  [DisallowMultipleComponent]
  public class PlayerSpawnerManagerAuthoring : MonoBehaviour
  {
    [Serializable]
    public struct PlayerSpawnerConfig
    {
      public Vector3 Location;
      public Vector3 Rotation;
      public bool    IsStartingSpawn;
    }

    public static PlayerSpawnerManagerAuthoring? Instance { get; private set; }

    [SerializeField] public GameObject?            PlayerPrefab;
    [SerializeField] public PlayerSpawnerConfig[]? SpawnerConfigs;

    private void Awake()
    {
      if (Instance is not null)
        Destroy(this);

      Instance = this;
    }

    private class Baker : Baker<PlayerSpawnerManagerAuthoring>
    {
      public override void Bake(PlayerSpawnerManagerAuthoring playerSpawnerManagerAuthoring)
      {
        if (playerSpawnerManagerAuthoring.PlayerPrefab is null)
        {
          Logger.Error(
            $"Player spawner `{playerSpawnerManagerAuthoring.name}` must have a player prefab associated with it"
          );
        }

        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new PlayerSpawnerInstanceComponent
        {
          Instance = entity
        });
        AddComponent(entity, new PlayerPrefabComponent
        {
          PlayerPrefab = GetEntity(playerSpawnerManagerAuthoring.PlayerPrefab!, TransformUsageFlags.Dynamic)
        });

        // Add a dynamic buffer to hold all of the entity's relationships to each spawner component struct
        var playerSpawnerComponentBuffer = AddBuffer<PlayerSpawnerComponent>(entity);
        if (playerSpawnerManagerAuthoring.SpawnerConfigs is null)
          return;

        foreach (var playerSpawnerConfig in playerSpawnerManagerAuthoring.SpawnerConfigs)
        {
          playerSpawnerComponentBuffer.Add(new PlayerSpawnerComponent
          {
            Location        = playerSpawnerConfig.Location,
            Rotation        = playerSpawnerConfig.Rotation,
            IsStartingSpawn = playerSpawnerConfig.IsStartingSpawn
          });
        }
      }
    }
  }
}