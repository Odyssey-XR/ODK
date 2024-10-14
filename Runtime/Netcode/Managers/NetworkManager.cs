using System;
using System.Diagnostics.CodeAnalysis;
using ODK.Netcode.Collections;
using ODK.Netcode.Managers.Interfaces;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;

namespace ODK.Netcode.Managers
{
  [ExcludeFromCodeCoverage]
  public class NetworkManager : INetworkManager
  {
    private Unity.Netcode.NetworkManager _networkManager => Unity.Netcode.NetworkManager.Singleton;

    public void StartHost()
    {
      _networkManager.StartHost();
    }

    public void StartServer()
    {
      _networkManager.StartServer();
    }

    public void StartClient()
    {
      _networkManager.StartClient();
    }

    public void LoadSceneForAllConnections(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
      _networkManager.SceneManager.LoadScene(sceneName, mode);
    }

    public void SetRelayServerData(RelayData relayData)
    {
      UnityTransport unityTransport = _networkManager.GetComponent<UnityTransport>();
      if (unityTransport is null)
        throw new Exception("Cannot find unity transport component.");

      unityTransport.SetRelayServerData(
        ipv4Address: relayData.IPV4Address,
        port: relayData.Port,
        allocationIdBytes: relayData.AllocationId,
        keyBytes: relayData.Key,
        connectionDataBytes: relayData.ConnectionData,
        isSecure: relayData.IsSecure);
    }
    
    public void SetRelayClientData(RelayData relayData)
    {
      UnityTransport unityTransport = _networkManager.GetComponent<UnityTransport>();
      if (unityTransport is null)
        throw new Exception("Cannot find unity transport component.");

      unityTransport.SetClientRelayData(
        ipAddress: relayData.IPV4Address,
        port: relayData.Port,
        allocationId: relayData.AllocationId,
        key: relayData.Key,
        connectionData: relayData.ConnectionData,
        hostConnectionData: relayData.HostConnectionData,
        isSecure: relayData.IsSecure);
    }
  }
}