namespace ChatMessagesApp.Core.Application.Interfaces;

public interface IUserConnectionManager
{
    void AddConnection(string userId, string connectionId);
    void RemoveConnection(string userId, string connectionId);
    IEnumerable<string> GetConnections(string userId);
}
