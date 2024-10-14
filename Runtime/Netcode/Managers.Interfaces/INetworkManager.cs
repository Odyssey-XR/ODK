using ODK.Netcode.Collections;
using UnityEngine.SceneManagement;

namespace ODK.Netcode.Managers.Interfaces
{
  public interface INetworkManager
  {
    void StartHost();
    void StartServer();
    void StartClient();
    void SetRelayServerData(RelayData relayData);
    void SetRelayClientData(RelayData relayData);
    void LoadSceneForAllConnections(string sceneName, LoadSceneMode mode = LoadSceneMode.Single);
  }
}