namespace SiegeInitiative.Application.Options.Base;

/// <summary>
/// Distributed cache options
/// </summary>
public interface IRedisCacheOptions
{
    /// <summary>
    /// Cache enable flag
    /// </summary>
    bool Enabled { get; }

    /// <summary>
    /// Distributed Cache Service Connection String
    /// </summary>
    string ConnectionString { get; }

    /// <summary>
    /// Cache service registration expiration time
    /// </summary>
    TimeSpan ExpirationTime { get; }
}