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

  public class ConnectionManager : MonoBehaviour
  {
    [SerializeField] private TMP_Dropdown?   _connectionModeDropdown;
    [SerializeField] private TMP_InputField? _addressField;
    [SerializeField] private TMP_InputField? _portField;
    [SerializeField] private Button?         _connectButton;

    public string Address => _addressField?.text ?? "127.0.0.1";
    public ushort Port    => ushort.Parse(_portField?.text ?? "7979");

    private void OnEnable()
    {
      _connectButton?.onClick.AddListener(OnButtonConnect);
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
          StartClient();
          StartServer();
          break;
        case 1:
          StartServer();
          break;
        case 2:
          StartClient();
          break;
        default:
          Core.Logging.Logger.Error($"Error: Unknown connection mode {_connectionModeDropdown?.value}");
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

      var serverEndpoint = NetworkEndpoint.Parse(Address, Port);

      using var query = serverWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
      query.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(serverEndpoint);
    }

    private void StartClient()
    {
      var clientWorld = ClientServerBootstrap.CreateClientWorld("ODK Client World");
      World.DefaultGameObjectInjectionWorld = clientWorld;

      var clientEndpoint = NetworkEndpoint.Parse(Address, Port);

      using var query = clientWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
      query.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(clientWorld.EntityManager, clientEndpoint);
    }
  }
}