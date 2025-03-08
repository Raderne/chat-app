using ChatMessagesApp.Core.Application.Interfaces;
using ChatMessagesApp.Core.Domain.Common;
using ChatMessagesApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace ChatMessagesApp.Infrastructure.Context;

public class IdentityContext(
    ICurrentUserService currentUserService,
    DbContextOptions<IdentityContext> options) : IdentityDbContext<AppUser, ApplicationRole, string, ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>(options), IIdentityContext
{
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private IDbContextTransaction? _currentTransaction;

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction != null) return;

        _currentTransaction = await base.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted)
            .ConfigureAwait(false);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default, string TenantId = null)
    {
        try
        {
            await SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            _currentTransaction?.Commit();
        }
        catch (Exception)
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null!;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null!;
            }
        }
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return SaveChangesAsync(null, cancellationToken);
    }

    public async Task<int> SaveChangesAsync(Action<Exception> onException = null, CancellationToken cancellationToken = default, string TenantId = null)
    {
        string userId = null!;
        if (_currentUserService.IsLoggedIn())
        {
            userId = $"{_currentUserService.UserName}:{_currentUserService.UserId}";
        }

        // Apply auditing 
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity<string>>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.Created = DateTime.Now;
                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModified = DateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModified = DateTime.Now;
                    break;
            }
        }

        var result = 0;
        try
        {
            result = await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            if (onException != null)
            {
                onException(ex);
            }
            else
            {
                throw new ApplicationException($"An error occured while saving data: {ex.Message}");
            }
        }

        return result;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>(b =>
        {
            // Each User can have many UserClaims
            b.HasMany(e => e.Claims)
                .WithOne(e => e.User)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            // Each User can have many UserLogins
            b.HasMany(e => e.Logins)
                .WithOne(e => e.User)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // Each User can have many UserTokens
            b.HasMany(e => e.Tokens)
                .WithOne(e => e.User)
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            // Each User can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });

        builder.Entity<ApplicationRole>(b =>
        {
            // Each Role can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            // Each Role can have many associated RoleClaims
            b.HasMany(e => e.RoleClaims)
                .WithOne(e => e.Role)
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();
        });

        var roles = new List<ApplicationRole>
        {
            new ApplicationRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
            new ApplicationRole { Id = "2", Name = "User", NormalizedName = "USER" }
        };
        builder.Entity<ApplicationRole>().HasData(roles);

    }
}
