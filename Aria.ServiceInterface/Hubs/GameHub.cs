using Aria.ServiceModel.Types.Players;
using Aria.ServiceModel.Types.Players.Server;
using Microsoft.AspNet.SignalR;
using System;
using System.Threading.Tasks;

namespace Aria.ServiceInterface.Hubs;

public class GameHub<PlayerType> : PlayerHubBase<PlayerType> where PlayerType : Player
{
    public GameHub(IPlayerRepository serverPlayerRepository) : base(serverPlayerRepository)
    {
    }
}
