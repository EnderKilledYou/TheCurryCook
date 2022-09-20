using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aria.ServiceModel.Types.Game.Server;

public interface IGameRepository
{
    Task CopyAll(List<IGame> games);

    Task AddGame(IGame player);
    Task RemoveGame(IGame game, Exception? exception = null);
}
