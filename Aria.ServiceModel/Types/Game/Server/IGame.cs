using Aria.ServiceModel.Types.Players.Server;
using System;
using System.Threading.Tasks;

namespace Aria.ServiceModel.Types.Game.Server
{
    public interface IGame : IPlayerRepository, IGuidCarrier
    {
     
        Task Render(); //sent on a fixed interval to all clients
        Task Start();
        Task Stop();

    }
}