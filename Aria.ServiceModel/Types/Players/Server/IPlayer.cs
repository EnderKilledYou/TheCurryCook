using Aria.ServiceModel.Types.Channel.Server;
using Aria.ServiceModel.Types.Game.Server;
using Aria.ServiceModel.Types.Lobby.Server;
using System.Threading.Tasks;

namespace Aria.ServiceModel.Types.Players.Server
{
    public interface IPlayer
    {
        GuidHoarder<IChannel> Channels { get; set; }
        string ConnectionId { get; set; }
        GuidHoarder<IGame> Games { get; set; }
        GuidHoarder<ILobby> Lobbies { get; set; }

        Task OnConnect();
        Task OnDisconnect();
        Task OnReconnect();
    }
}