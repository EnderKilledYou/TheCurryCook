using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aria.ServiceModel.Types.Players.Server;

public interface IPlayerRepository
{
    Task CopyAll(List<Player> players);
    Task<Player> GetByConnectionId(string connectionId);
    Task AddPlayer(Player player);
    Task RemovePlayer(Player player, Exception? exception = null);
    Task UpdatePlayerConnectionId(Guid playerId, string connectionId);
}
