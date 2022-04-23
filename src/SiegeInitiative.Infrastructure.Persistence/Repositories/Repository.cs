using Microsoft.EntityFrameworkCore;
using SiegeInitiative.Domain.Entities.Base;
using SiegeInitiative.Domain.Persistence.Repositories.Base;
using SiegeInitiative.Infrastructure.Persistence.Core;
using System.Linq.Expressions;

namespace SiegeInitiative.Infrastructure.Persistence.Repositories;

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : AggregateRoot<TKey>
{
    #region Fields

    private readonly SiegeDbContext dbContext;
    public IUnitOfWork UnitOfWork { get => dbContext; }
    protected DbSet<TEntity> Dbset => dbContext.Set<TEntity>();

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructo
    /// </summary>
    /// <param name="dbContext"></param>
    public Repository(SiegeDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    #endregion

    #region IRepository methods

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var added = await Dbset.AddAsync(entity, cancellationToken);

        return added.Entity;
    }

    public void Delete(TEntity entity)
        => Dbset.Remove(entity);

    public TEntity Update(TEntity entity)
    {
        var updated = Dbset.Update(entity);

        return updated.Entity;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await Dbset.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
    {
        var query = BuildQuery(includes);

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
    {
        var query = BuildQuery(includes);

        return await query.Where(expression).FirstOrDefaultAsync();
    }

    public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
    {
        var query = BuildQuery(includes);

        return await query.Where(expression).SingleOrDefaultAsync();
    }

    #endregion

    #region Private methods

    private IQueryable<TEntity> BuildQuery(params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = Dbset;

        foreach (var include in includes)
            query = query.Include(include);

        return query;
    }

    #endregion
}