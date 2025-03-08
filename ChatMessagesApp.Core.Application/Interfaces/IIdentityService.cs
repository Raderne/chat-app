using ChatMessagesApp.Core.Application.Features;
using ChatMessagesApp.Core.Application.Models;
using ChatMessagesApp.Core.Domain.DataObj;
using Microsoft.AspNetCore.Identity;

namespace ChatMessagesApp.Core.Application.Interfaces;

public interface IIdentityService
{
    Task<(Result Result, string UserId)> CreateUserAsync(User user, string password);
    Task<SignInResult> Loginuser(User user, string password);
    Task<string> GenerateTokenAsync(User user);
    Task<User> GetUserByEmailAsync(string email);
    Task<List<GetUsersDto>> GetAllUsers(CancellationToken cancellationToken);
}

