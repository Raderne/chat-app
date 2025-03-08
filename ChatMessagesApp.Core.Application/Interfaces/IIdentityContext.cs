namespace ChatMessagesApp.Core.Application.Interfaces;

public interface IIdentityContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    Task<int> SaveChangesAsync(Action<Exception> onException = null, CancellationToken cancellationToken = new CancellationToken(), string TenantId = null);
    Task CommitTransactionAsync(CancellationToken cancellationToken = new CancellationToken(), string TenantId = null);
    Task BeginTransactionAsync();
    void RollbackTransaction();
}

