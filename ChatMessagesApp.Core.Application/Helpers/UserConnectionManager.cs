using ChatMessagesApp.Core.Application.Interfaces;
using System.Collections.Concurrent;

namespace ChatMessagesApp.Core.Application.Helpers;

public class UserConnectionManager : IUserConnectionManager
{
    private readonly ConcurrentDictionary<string, HashSet<string>> _connections = new();

    public void AddConnection(string userId, string connectionId)
    {
        _connections.AddOrUpdate(userId,
            new HashSet<string> { connectionId },
            (_, set) => { set.Add(connectionId); return set; });
    }

    public IEnumerable<string> GetConnections(string userId) =>
        _connections.TryGetValue(userId, out var set) ? set : Enumerable.Empty<string>();

    public void RemoveConnection(string userId, string connectionId)
    {
        if (_connections.TryGetValue(userId, out var set))
        {
            set.Remove(connectionId);
            if (set.Count == 0) _connections.TryRemove(userId, out _);
        }
    }
}

//   dotnet add package StackExchange.Redis

//   using ChatMessagesApp.Core.Application.Interfaces;
//   using StackExchange.Redis;
//   using System.Collections.Generic;

//   namespace ChatMessagesApp.Core.Application.Helpers
//{
//    public class UserConnectionManager : IUserConnectionManager
//    {
//        private readonly IDatabase _database;

//        public UserConnectionManager(IConnectionMultiplexer connectionMultiplexer)
//        {
//            _database = connectionMultiplexer.GetDatabase();
//        }

//        public void AddConnection(string userId, string connectionId)
//        {
//            _database.SetAdd(userId, connectionId);
//        }

//        public IEnumerable<string> GetConnections(string userId)
//        {
//            return _database.SetMembers(userId).ToStringArray();
//        }

//        public void RemoveConnection(string userId, string connectionId)
//        {
//            _database.SetRemove(userId, connectionId);
//            if (_database.SetLength(userId) == 0)
//            {
//                _database.KeyDelete(userId);
//            }
//        }
//    }
//}
