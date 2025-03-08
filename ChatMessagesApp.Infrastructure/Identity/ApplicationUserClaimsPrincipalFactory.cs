using ChatMessagesApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ChatMessagesApp.Infrastructure.Identity;

public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser>
{
    RoleManager<ApplicationRole> _roleManager;
    public ApplicationUserClaimsPrincipalFactory(
        UserManager<AppUser> userManager,
        IOptions<IdentityOptions> optionsAccessor, RoleManager<ApplicationRole> roleManager)
        : base(userManager, optionsAccessor)
    {
        _roleManager = roleManager;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        List<Claim> claims = new List<Claim>();

        claims.Add(new Claim(ClaimTypes.GivenName, $"{user.LastName} {user.FirstName}"));

        if (UserManager.SupportsUserRole)
        {
            IList<string> roles = await UserManager.GetRolesAsync(user);
            foreach (var roleName in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
                if (_roleManager.SupportsRoleClaims)
                {
                    ApplicationRole role = await _roleManager.FindByNameAsync(roleName);
                    if (role != null)
                    {
                        claims.AddRange(await _roleManager.GetClaimsAsync(role));
                    }
                }
            }
        }

        identity.AddClaims(claims);

        return identity;
    }
}
