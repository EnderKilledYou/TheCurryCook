using Aria.ServiceModel.Types.Lobby.Server;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Aria.ServiceModel.Types.Players.Server;

namespace Aria.ServiceModel.Types.Channel.Server;

public interface IChannel : IGuidCarrier
{

    Task SendMessage(IPlayer from, IChannelMessage message);
    Task Join(IPlayer player);
    Task Leave(IPlayer player);

}
