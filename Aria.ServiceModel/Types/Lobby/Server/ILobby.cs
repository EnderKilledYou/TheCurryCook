using Aria.ServiceModel.Types.Players.Server;
using System.Threading.Tasks;

namespace Aria.ServiceModel.Types.Lobby.Server;

public interface ILobby
{
    Task SendMessage(Player from, ILobbyMessage message);
    Task Join(Player player);
    Task Leave(Player player);
    Task<bool> LobbyReady();
    Task<bool> Ready(Player player);
    Task<Player[]> Ready();
    Task<Player[]> NotReady();
}
