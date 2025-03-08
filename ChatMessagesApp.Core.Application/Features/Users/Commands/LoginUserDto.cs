namespace ChatMessagesApp.Core.Application.Features.Users.Commands;

public class LoginUserDto
{
    public string Token { get; set; } = null!;
    public string UserName { get; set; } = null!;
}
