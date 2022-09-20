using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Aria.ServiceModel.Types.Players.Server;

public abstract class Player
{


    public Guid Id { get; set; }
    public string ConnectionId { get; set; }


    protected abstract HubClientType ClientInstance<HubClientType>() where HubClientType : IPlayerClient;
    public abstract Task OnDisconnect();
    public abstract Task OnReconnect();
    public abstract Task OnConnect();

    protected Player(string connectionId)
    {
        ConnectionId = connectionId;
    }


}
