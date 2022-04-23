using SiegeInitiative.Domain.Entities.Base;

namespace SiegeInitiative.Application.Caching.Base;

/// <summary>
/// Redis communication interface 
/// </summary>
public interface IRedisCacheService<TEntity, TKey> where TEntity : AggregateRoot<TKey>
{
    /// <summary>
    /// Persist an object in redis
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cacheKey"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    Task SetAsync(TEntity entity, string cacheKey, CancellationToken cancellation = default);

    /// <summary>
    /// Returns a redis record according to the given key
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    Task<TEntity> GetAsync(string cacheKey, CancellationToken cancellation = default);
}