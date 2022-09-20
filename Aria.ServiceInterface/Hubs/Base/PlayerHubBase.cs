using Aria.ServiceModel.Types.Game.Server;
using Aria.ServiceModel.Types.Lobby.Server;
using Aria.ServiceModel.Types.Players;
using Aria.ServiceModel.Types.Players.Server;
using Microsoft.AspNet.SignalR;
using System;
using System.Threading.Tasks;

namespace Aria.ServiceInterface.Hubs.Base
{
    public abstract class PlayerHubBase<PlayerType> : Hub<IPlayerClient> where PlayerType : Player
    {
        public PlayerHubBase(IPlayerRepository<PlayerType> serverPlayerRepository, ILobbyRepository lobbyRepository, IGameRepository games)
        {
            this.serverPlayerRepository = serverPlayerRepository;
            this.lobbyRepository = lobbyRepository;
            this.games = games;
        }
        private readonly IPlayerRepository<PlayerType> serverPlayerRepository;
        private readonly ILobbyRepository lobbyRepository;
        private readonly IGameRepository games;

        public override async Task OnConnected()
        {
            PlayerType p = await serverPlayerRepository.AddPlayer(Context.ConnectionId);
            await p.OnConnect();
            await base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            if (serverPlayerRepository.GetByConnectionId(Context.ConnectionId) is PlayerType player)
            {
                player.OnDisconnect();
            }
            return base.OnDisconnected(stopCalled);
        }
        public override Task OnReconnected()
        {
            if (serverPlayerRepository.GetByConnectionId(Context.ConnectionId) is PlayerType player)
            {
                player.OnReconnect();
            }
            return base.OnReconnected();
        }
    }
}