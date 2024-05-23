namespace ODK.GameObjects.NetCode
{
  using Unity.Netcode;
  using UnityEngine;

  /// <summary>
  /// A network manager that automatically starts a host or client based on the build target.
  /// </summary>
  [RequireComponent(typeof(NetworkManager))]
  public class NetworkConnectionManager : MonoBehaviour
  {
    /// <inheritdoc cref="MonoBehaviour"/>
    protected void Start()
    {
#if UNITY_EDITOR
      NetworkManager.Singleton.StartHost();
#else
      NetworkManager.Singleton.StartClient();
#endif
    }
  }
}