using SiegeInitiative.Domain.Entities.Base;
using System.Linq.Expressions;

namespace SiegeInitiative.Domain.Persistence.Repositories.Base;

public interface IRepository<TEntity, TKey> where TEntity : AggregateRoot<TKey>
{
    IUnitOfWork UnitOfWork { get; }

    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    void Delete(TEntity entity);

    TEntity Update(TEntity entity);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes);
}