#nullable enable

namespace OdysseyXR.ODK.Behaviours
{
  using Unity.Entities;
  using UnityEngine;
  using OdysseyXR.ODK.Components;
  using OdysseyXR.ODK.Extensions;
  using Logger = OdysseyXR.ODK.Core.Logging.Logger;
 
  /// <summary>
  /// Manager class for player spawners. Holds references to potential spawn locations for a player
  /// </summary>
  [DisallowMultipleComponent]
  public class PlayerSpawnerManagerAuthoring : MonoBehaviour
  {
    public static PlayerSpawnerManagerAuthoring? Instance { get; private set; }

    [SerializeField] public GameObject? PlayerPrefab;
    [SerializeField] public PlayerSpawnerTransformComponent[]? Transforms;

    private void Awake()
    {
      if (Instance is not null)
        Destroy(this);

      Instance = this;
    }
  }

  public class PlayerSpawnerManagerBaker : Baker<PlayerSpawnerManagerAuthoring>
  {
    public override void Bake(PlayerSpawnerManagerAuthoring playerSpawnerManagerAuthoring)
    {
      if (playerSpawnerManagerAuthoring.PlayerPrefab is null)
      {
        Logger.Error(
          $"Player spawner `{playerSpawnerManagerAuthoring.name}` must have a player prefab associated with it"
        );
        return;
      }

      if (playerSpawnerManagerAuthoring.Transforms == null || playerSpawnerManagerAuthoring.Transforms.Length <= 0)
      {
        Logger.Error(
          $"Player spawner `{playerSpawnerManagerAuthoring.name}` must have at least one spawn location associated with it"
        );
        return;
      }

      var entity = GetEntity(TransformUsageFlags.None);

      // Create the player spawner manager component
      AddComponent(
        entity,
        new PlayerSpawnerManagerComponent
        {
          Instance = entity,
          PlayerPrefab = GetEntity(playerSpawnerManagerAuthoring.PlayerPrefab!, TransformUsageFlags.Dynamic),
        }
      );
      
      // Create a dynamic buffer to hold all the spawner transforms
      this.AddBuffer(playerSpawnerManagerAuthoring.Transforms, entity);
    }
  }
}