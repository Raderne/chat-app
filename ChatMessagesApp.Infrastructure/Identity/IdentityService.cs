using ChatMessagesApp.Core.Application.Features;
using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Application.Models;
using ChatMessagesApp.Core.Domain.DataObj;
using ChatMessagesApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatMessagesApp.Infrastructure.Identity;

public class IdentityService(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IConfiguration configuration
    ) : IIdentityService
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly SignInManager<AppUser> _signInManager = signInManager;

    public async Task<(Result Result, string UserId)> CreateUserAsync(User user, string password)
    {
        var appUser = MapUser(user);

        appUser.Created = DateTime.UtcNow;
        appUser.EmailConfirmed = true;

        var result = await _userManager.CreateAsync(appUser, password);
        if (result.Succeeded)
        {
            foreach (var roleName in user.RoleNames)
            {
                await _userManager.AddToRoleAsync(appUser, roleName);
            }
        }
        else
        {
            throw new Exception(result.Errors.First().Description);
        }

        return (result.ToApplicationResult(), appUser.Id);
    }

    public async Task<List<GetUsersDto>> GetAllUsers(CancellationToken cancellationToken)
    {
        return await _userManager.Users
                                .Select(u => new GetUsersDto
                                {
                                    Id = u.Id,
                                    Email = u.Email,
                                    UserName = u.UserName,
                                    FirstName = u.FirstName,
                                    LastName = u.LastName,
                                    Created = u.Created
                                }).ToListAsync(cancellationToken);
    }

    public async Task<string> GenerateTokenAsync(User user)
    {
        var AppUser = MapUser(user);

        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("fullName", user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.Role, (await userManager.GetRolesAsync(AppUser)).FirstOrDefault()!.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id!),
            };

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
        catch (Exception)
        {
            return null!;
        }
    }

    public async Task<SignInResult> Loginuser(User user, string password)
    {
        var appUser = MapUser(user);
        appUser.NormalizedEmail = user.NormalizedEmail;
        appUser.NormalizedUserName = user.NormalizedUserName;

        return await _signInManager.PasswordSignInAsync(appUser.UserName!, password, false, false);
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        ArgumentNullException.ThrowIfNull(nameof(User), email);

        return MapAppUser(user);
    }

    private User MapAppUser(AppUser? user)
    {
        if (user == null) return null!;

        return new User()
        {
            Id = user.Id,
            Email = user.Email!,
            UserName = user.UserName!,
            FirstName = user.FirstName,
            NormalizedUserName = user.NormalizedUserName,
            NormalizedEmail = user.NormalizedEmail,
            LastName = user.LastName,
            RoleNames = new List<string>(),
            Created = user.Created,
            CreatedBy = user.CreatedBy,
            LastModified = user.LastModified,
            LastModifiedBy = user.LastModifiedBy,
            IsDeleted = user.IsDeleted,
        };
    }
    private static AppUser MapUser(User user)
    {
        return new AppUser()
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
        };
    }

}
