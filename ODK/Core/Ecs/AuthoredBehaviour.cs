#nullable enable

namespace OdysseyXR.Arcadia.Plugins.ODK.Core.Ecs
{
  using Unity.Entities;
  using UnityEngine;

  public abstract class AuthoredBehaviour : MonoBehaviour
  {
    public Entity AuthoredEntity;
  }
}