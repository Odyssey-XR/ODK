using System.Threading.Tasks;

namespace ODK.Netcode.Services.Interfaces
{
  public interface ILobbyService
  {
    Task CreateLobbyAsHostAsync(string lobbyName, int maxPlayers);

    Task QuickJoinLobbyAsClientAsync();
  }
}