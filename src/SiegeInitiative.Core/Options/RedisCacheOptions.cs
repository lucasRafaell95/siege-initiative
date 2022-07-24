using SiegeInitiative.Core.Options.Base;

namespace SiegeInitiative.Core.Options;

public sealed record RedisCacheOptions : IRedisCacheOptions
{
    /// <summary>
    /// Cache enable flag
    /// </summary>
    public bool Enabled { get; init; }

    /// <summary>
    /// Distributed Cache Service Connection String
    /// </summary>
    public string ConnectionString { get; init; }

    /// <summary>
    /// Cache service registration expiration time
    /// </summary>
    public TimeSpan ExpirationTime { get; init; }
}