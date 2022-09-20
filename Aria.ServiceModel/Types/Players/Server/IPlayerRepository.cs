using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aria.ServiceModel.Types.Players.Server;
public interface IPlayerRepository 
{
    Task CopyAll(List<IPlayer> players);
    Task<IPlayer> GetByGuid(Guid guid);
    Task<IPlayer> GetByConnectionId(string connectionId);
    Task<IPlayer> AddPlayer( Guid guid);
    Task RemovePlayer(IPlayer player, Exception? exception = null);
    Task UpdatePlayerConnectionId(Guid playerId, string connectionId);
}
