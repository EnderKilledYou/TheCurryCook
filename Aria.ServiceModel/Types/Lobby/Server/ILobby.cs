using Aria.ServiceModel.Types.Channel.Server;
using Aria.ServiceModel.Types.Players.Server;
using System;
using System.Threading.Tasks;

namespace Aria.ServiceModel.Types.Lobby.Server;

public interface ILobby : IChannel
{
    
    Task SendMessage(IPlayer from, ILobbyMessage message);

    Task<bool> LobbyReady();
    Task<bool> Ready(IPlayer player);
    Task<IPlayer[]> Ready();
    Task<IPlayer[]> NotReady();
}
