using System.Threading.Tasks;
using Aria.ServiceModel.Types.Players.Server;

namespace Aria.ServiceModel.Game;

/// <summary>
/// Handles 
/// </summary>
public interface IGameServer : IPlayerRepository
{
    Task Render(); //sent on a fixed interval to all clients
    Task Start();
    Task Stop();


}
