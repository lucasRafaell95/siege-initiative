using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SiegeInitiative.Core.Options.Base;
using System.Text;

namespace SiegeInitiative.Core.Caching;

public abstract class RedisCacheService
{
    #region Fields

    private readonly ILogger logger;
    protected readonly IDistributedCache cache;
    protected readonly IRedisCacheOptions options;

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="cache"></param>
    /// <param name="options"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public RedisCacheService(ILogger logger, IDistributedCache cache, IRedisCacheOptions options)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        this.options = options ?? throw new ArgumentNullException(nameof(options));
    }

    #endregion

    #region Protected methods

    /// <summary>
    /// Persist an object in redis
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cacheKey"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    protected virtual async Task SetAsync<TEntity>(TEntity entity, string cacheKey, CancellationToken cancellation = default)
    {
        try
        {
            if (!options.Enabled)
            {
                this.logger.LogInformation("Cache Disabled! The register will not be stored");

                return;
            }

            if (entity is null)
            {
                this.logger.LogError("The informed entity is null and will not be placed in the cache");

                return;
            }

            var json = SerializeContent(entity);
            var content = Encoding.UTF8.GetBytes(json);

            this.logger.LogInformation($"Inserting the value into the cache with the key {cacheKey}");

            await cache.SetAsync(cacheKey, content, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = options.ExpirationTime
            }, cancellation);
        }
        catch (Exception ex)
        {
            this.logger.LogError("Error trying to set value in redis", new
            {
                key = cacheKey,
                Exception = ex
            });
        }
    }

    /// <summary>
    /// Returns a redis record according to the given key
    /// </summary>
    /// <param name="cacheKey"></param>
    /// <param name="fallback"></param>
    /// <param name="cancellation"></param>
    /// <returns></returns>
    protected virtual async Task<TEntity> GetAsync<TEntity>(string cacheKey, Func<Task<TEntity>> fallback, CancellationToken cancellation = default)
    {
        TEntity? result = default;

        try
        {
            if (!options.Enabled)
            {
                this.logger.LogInformation("Cache Disabled! The redis search will not be performed");

                return result;
            }

            var value = await cache.GetAsync(cacheKey, cancellation);

            if (value is null)
            {
                result = await fallback();

                if (result != null)
                {
                    await SetAsync(result, cacheKey, cancellation);
                }

                return result;
            }

            result = DeserializeContent<TEntity>(value);
        }
        catch (Exception ex)
        {
            this.logger.LogError("An error occurred while trying to retrieve the cache information", new
            {
                CacheKey = cacheKey,
                Exception = ex
            });
        }

        return result;
    }

    #endregion

    #region Private Methods

    private static string SerializeContent<TEntity>(TEntity entity)
        => JsonConvert.SerializeObject(entity, Formatting.Indented, GetSerializerSettings());
    private static TEntity DeserializeContent<TEntity>(byte[] cachedValue)
        => JsonConvert.DeserializeObject<TEntity>(Encoding.UTF8.GetString(cachedValue), GetSerializerSettings());

    private static JsonSerializerSettings GetSerializerSettings()
        => new()
        {
            TypeNameHandling = TypeNameHandling.Objects,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        };

    #endregion
}