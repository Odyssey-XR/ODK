#nullable enable

namespace Plugins.ODK.Behaviours.XR
{
  using System;
  using Unity.Entities;
  using Unity.Transforms;
  using UnityEngine;

  public class XRCamera : MonoBehaviour
  {
    private EntityManager _entityManager;

    [NonSerialized]
    public GameObject? FloorOriginParent;

    public Entity? FloorOriginEntity = null;
    public Entity? HMDEntity         = null;

    public void Awake()
    {
      _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    public void Update()
    {
      if (World.DefaultGameObjectInjectionWorld.Flags != WorldFlags.GameClient || HMDEntity is null)
        return;

      if (FloorOriginEntity is not null)
        ConfigureFloorOrigin();

      var targetTransform = _entityManager.GetComponentData<LocalTransform>(HMDEntity.Value);
      transform.localPosition = targetTransform.Position;
      transform.localRotation = targetTransform.Rotation;
    }

    protected void ConfigureFloorOrigin()
    {
      if (FloorOriginParent is null)
      {
        FloorOriginParent = new GameObject { name = "Camera Floor Origin" };
        transform.parent  = FloorOriginParent.transform;
      }

      var targetTransform = _entityManager.GetComponentData<LocalToWorld>(FloorOriginEntity!.Value);
      FloorOriginParent.transform.localPosition = targetTransform.Position;
      FloorOriginParent.transform.localRotation = targetTransform.Rotation;
    }
  }
}