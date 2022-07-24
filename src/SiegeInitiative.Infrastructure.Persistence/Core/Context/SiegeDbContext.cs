using Microsoft.EntityFrameworkCore;
using SiegeInitiative.Domain.Persistence.Repositories.Base;

namespace SiegeInitiative.Infrastructure.Persistence.Core;

public sealed class SiegeDbContext : DbContext, IUnitOfWork
{
    public SiegeDbContext(DbContextOptions<SiegeDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(typeof(SiegeDbContext).Assembly);

    async Task<bool> IUnitOfWork.CommitTransactionAsync(CancellationToken cancellationToken)
    {
        var commited = await SaveChangesAsync(cancellationToken);

        return commited > 0;
    }
}