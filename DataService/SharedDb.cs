using System.Collections.Concurrent;
using Chat.Models;

namespace Chat.DataService;

public class SharedDb
{
    private readonly ConcurrentDictionary<string, UserConnection> _connections = new();
    public ConcurrentDictionary<string, UserConnection> Connections => _connections;
}