using Chat.DataService;
using Chat.Models;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Hubs;

public class ChatHub : Hub
{
    private readonly SharedDb _sharedDb;
    public ChatHub(SharedDb sharedDb)
    {
        _sharedDb = sharedDb;
    }
    public async Task JoinChat(UserConnection userConnection)
    {
        await Clients.All
            .SendAsync("ReceiveMessage","admin", $"{userConnection.Username} has joined the chat");
    }
    public async Task JoinSpecificChatRoom(UserConnection userConnection)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.ChatRoom);
        _sharedDb.Connections[Context.ConnectionId] = userConnection;
        await Clients.Group(userConnection.ChatRoom)
            .SendAsync("JoinSpecificChatRoom", "admin", $"{userConnection.Username} has joined the chat");
    }
    public async Task SendMessage(string message)
    {
        if(_sharedDb.Connections.TryGetValue(Context.ConnectionId, out var userConnection))
        {
            await Clients.Group(userConnection.ChatRoom)
                .SendAsync("ReceiveSpecificMessage", userConnection.Username, message);
        }
    }
}