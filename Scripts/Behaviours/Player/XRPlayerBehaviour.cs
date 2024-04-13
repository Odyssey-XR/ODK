#nullable enable

namespace Plugins.ODK.Behaviours.Player
{
  using OdysseyXR.ODK.Components.Player;
  using Plugins.ODK.Components.XR;
  using Unity.Entities;
  using UnityEngine;

  public class XRPlayerBehaviour : MonoBehaviour
  {
  }

  public class XRPlayerBaker : Baker<XRPlayerBehaviour>
  {
    public override void Bake(XRPlayerBehaviour authoring)
    {
      var entity = GetEntity(TransformUsageFlags.None);
      AddComponent<PlayerTag>(entity);
      AddComponent<GhostDeviceTransformInputComponent>(entity);
      AddComponent<GhostControllerActionInputComponent>(entity);
    }
  }
}