using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Aria.ServiceModel.Types;

public class GuidHoarder<GuidType> : ConcurrentDictionary<Guid, GuidType> where GuidType : IGuidCarrier
{
 
    public void Get(List<GuidType> lobbies)
    {
        lobbies.AddRange(Values);
    }
    public void Add(GuidType lobby)
    {
        TryAdd(lobby.GetGuid(), lobby);
    }
    public void Leave(Guid guid)
    {
        TryRemove(guid, out _);
    }
}
