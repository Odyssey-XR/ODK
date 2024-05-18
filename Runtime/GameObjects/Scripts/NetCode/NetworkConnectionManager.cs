namespace ODK.GameObjects.NetCode
{
  using Unity.Netcode;
  using UnityEngine;

  /// <summary>
  /// A network manager that automatically starts a host or client based on the build target.
  /// </summary>
  public class NetworkConnectionManager : NetworkManager
  {
    /// <inheritdoc cref="MonoBehaviour"/>
    protected void Start()
    {
#if UNITY_EDITOR
      StartHost();
#else
      StartClient();
#endif
    }
  }
}