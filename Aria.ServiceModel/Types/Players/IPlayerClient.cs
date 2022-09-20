using System.Threading.Tasks;
using Aria.ServiceModel.Types.Game.Server;
using Aria.ServiceModel.Types.Players.Server;

namespace Aria.ServiceModel.Types.Players;

public interface IPlayerClient
{
    Task SetGame(IGame game);
    Task SetScene(IGameScene scene);
    Task SetLevel(IGameLevel level);
    Task SetGameState(IGameRenderState state);
    Task SetTurn(IPlayerTurn turn);
    Task SendClientError(string v);
}
