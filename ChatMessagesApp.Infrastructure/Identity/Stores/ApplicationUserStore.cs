using ChatMessagesApp.Infrastructure.Context;
using ChatMessagesApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatMessagesApp.Infrastructure.Identity.Stores;

public class ApplicationUserStore : UserStore<AppUser, ApplicationRole,
        IdentityContext, string, ApplicationUserClaim, ApplicationUserRole,
        ApplicationUserLogin, ApplicationUserToken, ApplicationRoleClaim>
{
    public ApplicationUserStore(IdentityContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
    {
    }

    public override async Task<AppUser?> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        return await Users.IgnoreQueryFilters()
            .Include(e => e.Claims)
            .Include(e => e.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public override async Task<AppUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        return await Users.IgnoreQueryFilters()
            .Include(e => e.Claims)
            .Include(e => e.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);
    }
    public async override Task<AppUser?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        return await Users.IgnoreQueryFilters()
            .Include(e => e.Claims)
            .Include(e => e.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.NormalizedEmail == email, cancellationToken);
    }
}
