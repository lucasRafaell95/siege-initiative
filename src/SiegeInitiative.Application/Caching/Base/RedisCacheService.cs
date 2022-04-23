using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SiegeInitiative.Application.Options.Base;
using SiegeInitiative.Domain.Entities.Base;
using System.Text;

namespace SiegeInitiative.Application.Caching.Base;

public class RedisCacheService<TEntity, TKey> : IRedisCacheService<TEntity, TKey>
    where TEntity : AggregateRoot<TKey>
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

    #region IRedisCacheService methods

    public virtual async Task SetAsync(TEntity entity, string cacheKey, CancellationToken cancellation = default)
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

    public virtual async Task<TEntity> GetAsync(string cacheKey, CancellationToken cancellation = default)
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

            result = value is null
                ? default
                : DeserializeContent(value);
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

    private static string SerializeContent(TEntity entity)
        => JsonConvert.SerializeObject(entity, Formatting.Indented, GetSerializerSettings());
    private static TEntity DeserializeContent(byte[] cachedValue)
        => JsonConvert.DeserializeObject<TEntity>(Encoding.UTF8.GetString(cachedValue), GetSerializerSettings());

    private static JsonSerializerSettings GetSerializerSettings()
        => new()
        {
            TypeNameHandling = TypeNameHandling.Objects,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        };

    #endregion
}