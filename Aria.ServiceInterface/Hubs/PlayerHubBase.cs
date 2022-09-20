using Aria.ServiceModel.Types.Players;
using Aria.ServiceModel.Types.Players.Server;
using Microsoft.AspNet.SignalR;
using System;
using System.Threading.Tasks;

namespace Aria.ServiceInterface.Hubs
{
    public abstract class PlayerHubBase<PlayerType> : Hub<IPlayerClient> where PlayerType : Player
    {
        public PlayerHubBase(IPlayerRepository serverPlayerRepository)
        {
            this.serverPlayerRepository = serverPlayerRepository;
        }
        private readonly IPlayerRepository serverPlayerRepository;
        public override Task OnConnected()
        {
            if (Activator.CreateInstance(typeof(PlayerType), Context.ConnectionId) is PlayerType player)
            {
                serverPlayerRepository.AddPlayer(player);
                player.OnConnect();
            }
            else
            {

            }
            return base.OnConnected();
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