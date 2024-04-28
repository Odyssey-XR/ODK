#nullable enable

namespace OdysseyXR.ODK.Behaviours.NetCode
{
  using TMPro;
  using Unity.Entities;
  using Unity.NetCode;
  using Unity.Networking.Transport;
  using UnityEngine;
  using UnityEngine.SceneManagement;
  using UnityEngine.UI;
  using Logger = OdysseyXR.ODK.Services.Logging.Logger;

  public class ConnectionManager : MonoBehaviour
  {
    [SerializeField] private TMP_Dropdown?   _connectionModeDropdown;
    [SerializeField] private TMP_InputField? _addressField;
    [SerializeField] private TMP_InputField? _portField;
    [SerializeField] private Button?         _connectButton;

    public string ServerAddress = "127.0.0.1";
    public string ClientAddress = "192.168.0.9";
    public ushort Port    = ushort.Parse("7777");

    private void OnEnable()
    {
      _connectButton?.onClick.AddListener(OnButtonConnect);
      
      #if UNITY_ANDROID && !UNITY_EDITOR
      _connectionModeDropdown.value = 0;
      ClientAddress                 = ServerAddress;
      OnButtonConnect();
      #elif UNITY_EDITOR
      _connectionModeDropdown.value = 0;
      ClientAddress                 = ServerAddress;
      OnButtonConnect();
      #endif
    }

    private void OnDisable()
    {
      _connectButton?.onClick.RemoveAllListeners();
    }

    private void OnButtonConnect()
    {
      DestroyLocalSimulationWorld();
      SceneManager.LoadScene(1);

      switch (_connectionModeDropdown?.value)
      {
        case 0:
          StartServer();
          StartClient();
          break;
        case 1:
          StartServer();
          break;
        case 2:
          StartClient();
          break;
        default:
          Logger.Error($"Error: Unknown connection mode {_connectionModeDropdown?.value}");
          break;
      }
    }

    private static void DestroyLocalSimulationWorld()
    {
      foreach (var world in World.All)
      {
        if (world.Flags != WorldFlags.Game)
          continue;

        world.Dispose();
        break;
      }
    }

    private void StartServer()
    {
      var serverWorld = ClientServerBootstrap.CreateServerWorld("ODK Server World");
      World.DefaultGameObjectInjectionWorld = serverWorld;

      var serverEndpoint = NetworkEndpoint.AnyIpv4.WithPort(Port);

      using var query = serverWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
      query.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(serverEndpoint);
      Logger.Log("Started server world");
    }

    private void StartClient()
    {
      var clientWorld = ClientServerBootstrap.CreateClientWorld("ODK Client World");
      World.DefaultGameObjectInjectionWorld = clientWorld;

      var clientEndpoint = NetworkEndpoint.Parse(ClientAddress, Port);

      using var query = clientWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
      query.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(clientWorld.EntityManager, clientEndpoint);
      Logger.Log("Started client world");
    }
  }
}