namespace ChatMessagesApp.Core.Application.Interfaces;

public interface ICurrentUserService
{
    public string UserId { get; }
    public string UserName { get; }
    public string PreferedUserName { get; }
    List<string> Roles { get; }

    public bool IsLoggedIn()
    {
        return !string.IsNullOrEmpty(UserId);
    }
}

