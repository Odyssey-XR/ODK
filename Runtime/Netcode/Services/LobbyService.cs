using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Netcode.Constants;
using ODK.Netcode.Collections;
using ODK.Netcode.Managers.Interfaces;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay.Models;
using UnityEngine;
using ILobbyService = ODK.Netcode.Services.Interfaces.ILobbyService;
using UnityRelayService = Unity.Services.Relay.RelayService;
using UnityLobbyService = Unity.Services.Lobbies.LobbyService;

namespace ODK.Netcode.Services
{
  public class LobbyService : ILobbyService
  {
    private readonly INetworkManager _networkManager;

    public LobbyService(INetworkManager networkManager)
    {
      _networkManager = networkManager;
    }

    public async Task CreateLobbyAsHostAsync(string lobbyName, int maxPlayers)
    {
      Allocation allocation = await CreateAllocation(maxPlayers - 1);

      CreateLobbyOptions options = new() { IsPrivate = false };
      Lobby lobby = await UnityLobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

      string joinCode = await GetAllocationJoinCode(allocation.AllocationId);
      await UnityLobbyService.Instance.UpdateLobbyAsync(lobby.Id, new UpdateLobbyOptions()
      {
        Data = new Dictionary<string, DataObject>
        {
          { LobbyDataConstants.JoinCode, new DataObject(DataObject.VisibilityOptions.Public, joinCode) }
        }
      });

      RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.First(_ => _.ConnectionType == "dtls");
      RelayData relayData = new(
        ipv4Address: dtlsEndpoint.Host,
        port: (ushort)dtlsEndpoint.Port,
        allocationId: allocation.AllocationIdBytes,
        key: allocation.Key,
        connectionData: allocation.ConnectionData,
        isSecure: true);

      _networkManager.SetRelayServerData(relayData);
      _networkManager.StartHost();

      Debug.Log($"Created lobby {lobbyName} on allocation {allocation.AllocationId} - region { allocation.Region }");
    }

    public async Task QuickJoinLobbyAsClientAsync()
    {
      Lobby lobby = await UnityLobbyService.Instance.QuickJoinLobbyAsync();
      string joinCode = lobby.Data[LobbyDataConstants.JoinCode].Value;
      if (string.IsNullOrWhiteSpace(joinCode))
        throw new Exception($"No public join code found on lobby {lobby.Name}");

      JoinAllocation allocation = await JoinAllocation(joinCode);
      RelayServerEndpoint dtlsEndpoint = allocation.ServerEndpoints.First(_ => _.ConnectionType == "dtls");
      RelayData relayData = new(
        ipv4Address: dtlsEndpoint.Host,
        port: (ushort)dtlsEndpoint.Port,
        allocationId: allocation.AllocationIdBytes,
        key: allocation.Key,
        connectionData: allocation.ConnectionData,
        hostConnectionData: allocation.HostConnectionData,
        isSecure: dtlsEndpoint.Secure);
      
      _networkManager.SetRelayClientData(relayData);
      _networkManager.StartClient();
      
      Debug.Log($"Joined lobby {lobby.Name} on allocation {allocation.AllocationId} - region {allocation.Region}");
    }

    public async Task<Allocation> CreateAllocation(int maxPlayers)
    {
      return await UnityRelayService.Instance.CreateAllocationAsync(maxPlayers);
    }

    public async Task<JoinAllocation> JoinAllocation(string joinCode)
    {
      return await UnityRelayService.Instance.JoinAllocationAsync(joinCode);
    }

    public async Task<string> GetAllocationJoinCode(Guid allocationId)
    {
      return await UnityRelayService.Instance.GetJoinCodeAsync(allocationId);
    }
  }
}