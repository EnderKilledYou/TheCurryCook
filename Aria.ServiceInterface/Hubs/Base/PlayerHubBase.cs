using Aria.ServiceModel.Types;
using Aria.ServiceModel.Types.Game.Server;
using Aria.ServiceModel.Types.Lobby.Server;
using Aria.ServiceModel.Types.Players;
using Aria.ServiceModel.Types.Players.Server;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hosting;
using ServiceStack;
using ServiceStack.Auth;
using System;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Host;
using ServiceStack.Testing;
using HostContext = ServiceStack.HostContext;

namespace Aria.ServiceInterface.Hubs.Base
{
    public abstract class PlayerHubBase : Hub<IPlayerClient>
    {
        public PlayerHubBase(IPlayerRepository serverPlayerRepository, ILobbyRepository lobbyRepository, IGameRepository games, IAuthRepository repository)
        {
            this.serverPlayerRepository = serverPlayerRepository;
            this.lobbyRepository = lobbyRepository;
            this.games = games;
            this.repository = repository;
        }
        private readonly IPlayerRepository serverPlayerRepository;
        private readonly ILobbyRepository lobbyRepository;
        private readonly IGameRepository games;
        private readonly IAuthRepository repository;

        public override async Task OnConnected()
        {
            var token = Context.Request.Headers["Authorization"];
            if (token == null)
            {
                await Clients.Caller.SendClientError("Authentication Required");
                return;
            }

            if (GetCurrentUser() is IGuidCarrier usr)
            {
                var guid = usr.GetGuid();
                IPlayer p = await serverPlayerRepository.AddPlayer(guid);
                await p.OnConnected(Context.ConnectionId);

            }
            await base.OnConnected();
        }
        public override async Task OnDisconnected(bool stopCalled)
        {

            if (await serverPlayerRepository.GetByConnectionId(Context.ConnectionId) is IPlayer player)
            {
                await player.OnDisconnect(stopCalled);
            }
            await base.OnDisconnected(stopCalled);
        }
        public override async Task OnReconnected()
        {

            if (GetCurrentUser() is IGuidCarrier usr)
            {
                if (await serverPlayerRepository.GetByGuid(usr.GetGuid()) is IPlayer player)
                {
                    await player.OnReconnect(Context.ConnectionId);
                }
            }
            await base.OnReconnected();
        }

        private IUserAuth GetCurrentUser()
        {
            var sess = HostContext.GetCurrentRequest().SessionAs<AuthUserSession>();
            var user = repository.GetUserAuthByUserName(sess.UserName);
            return user;
        }
    }
}