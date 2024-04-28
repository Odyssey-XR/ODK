#nullable enable

namespace Plugins.ODK.Behaviours.Player
{
  using OdysseyXR.ODK.Attributes.ECS;
  using OdysseyXR.ODK.Components.Player;
  using UnityEngine;

  [AutoBaker]
  [BakeWithTags(typeof(PlayerTag))]
  public class XRPlayer : MonoBehaviour
  {
    [SerializeField]
    [BakeAsEntity]
    public GameObject? FloorOrigin;
      
    [SerializeField]
    [BakeAsEntity]
    public GameObject? HMDTracker;
  }
}