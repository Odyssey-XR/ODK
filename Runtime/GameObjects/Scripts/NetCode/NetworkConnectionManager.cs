namespace ODK.GameObjects.NetCode
{
  using Unity.Netcode;

  public class NetworkConnectionManager : NetworkManager
  {
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