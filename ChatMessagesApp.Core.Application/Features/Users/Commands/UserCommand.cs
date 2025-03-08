using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Domain.DataObj;
using MediatR;

namespace ChatMessagesApp.Core.Application.Features.Users.Commands;

public class UserCommand : IRequest<LoginUserDto>
{
    public string? UserName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? RoleName { get; set; }
    public string Password { get; set; } = string.Empty;
}

public class UserCommandHandler : IRequestHandler<UserCommand, LoginUserDto>
{
    private readonly IIdentityService _identityService;
    public UserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    public async Task<LoginUserDto> Handle(UserCommand request, CancellationToken cancellationToken)
    {

        var IsUserExist = await _identityService.GetUserByEmailAsync(request.Email);

        if (IsUserExist == null)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                RoleNames = new List<string> { request.RoleName }
            };

            var (result, userId) = await _identityService.CreateUserAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new Exception("User creation failed");
            }

            IsUserExist = user;
        }

        var signInResult = await _identityService.Loginuser(IsUserExist, request.Password);
        if (!signInResult.Succeeded)
        {
            throw new Exception("User login failed");
        }

        var token = await _identityService.GenerateTokenAsync(IsUserExist);
        if (string.IsNullOrEmpty(token))
        {
            throw new Exception("Token generation failed");
        }

        var userDto = new LoginUserDto
        {
            Token = token,
            UserName = IsUserExist.UserName!,
        };

        return userDto;
    }
}