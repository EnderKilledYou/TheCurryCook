using ServiceStack;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aria.ServiceModel;
public abstract class Player
{
    public Guid Id { get; set; }
    public string ConnectionId { get; set; }

    protected Player(string connectionId)
    {
        ConnectionId = connectionId;
    }

}
public interface IGameMessage
{

}
public interface IPlayerRepository
{
    Task BroadcastAll(IGameMessage gameMessage);
    Task AddPlayer(Player player);
    Task RemovePlayer(Player player, Exception? exception = null);
    Task UpdatePlayerConnectionId(Guid playerId, string connectionId);
}
/// <summary>
/// Handles player connections for the server
/// </summary>
public abstract class ServerPlayerRepository : IPlayerRepository
{

    public abstract Task AddPlayer(Player player);

    public abstract Task BroadcastAll(IGameMessage gameMessage);

    public abstract Task RemovePlayer(Player player, Exception? exception = null);

    public abstract Task UpdatePlayerConnectionId(Guid playerId, string connectionId);
}
/// <summary>
/// Handles 
/// </summary>
public interface IGameInstance : IPlayerRepository
{

    public Task Start();
    public Task Stop();


}



[Tag("todos")]
[Route("/todos", "GET")]
public class QueryTodos : QueryData<Todo>
{
    public int? Id { get; set; }
    public List<long>? Ids { get; set; }
    public string? TextContains { get; set; }
}

[Tag("todos")]
[Route("/todos", "POST")]
public class CreateTodo : IPost, IReturn<Todo>
{
    [ValidateNotEmpty]
    public string Text { get; set; }
}

[Tag("todos")]
[Route("/todos/{Id}", "PUT")]
public class UpdateTodo : IPut, IReturn<Todo>
{
    public long Id { get; set; }
    [ValidateNotEmpty]
    public string Text { get; set; }
    public bool IsFinished { get; set; }
}

[Tag("todos")]
[Route("/todos/{Id}", "DELETE")]
public class DeleteTodo : IDelete, IReturnVoid
{
    public long Id { get; set; }
}

[Tag("todos")]
[Route("/todos", "DELETE")]
public class DeleteTodos : IDelete, IReturnVoid
{
    public List<long> Ids { get; set; }
}

public class Todo : IHasId<long>
{
    public long Id { get; set; }
    public string Text { get; set; }
    public bool IsFinished { get; set; }
}
