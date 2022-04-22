namespace SiegeInitiative.Domain.Persistence.Repositories.Base;

public interface IUnitOfWork
{
    Task<bool> CommitTransactionAsync(CancellationToken cancellationToken = default);
}