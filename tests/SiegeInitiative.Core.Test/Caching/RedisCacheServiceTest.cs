using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using SiegeInitiative.Core.Caching;
using SiegeInitiative.Core.Caching.Base;
using SiegeInitiative.Core.Options.Base;
using SiegeInitiative.Domain.Entities;
using SiegeInitiative.Domain.Entities.Base;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SiegeInitiative.Core.Test.Caching;

public sealed class RedisCacheServiceTest
{
    private readonly Mock<ILogger> logger = new();
    private readonly Mock<IDistributedCache> cache = new();
    private readonly Mock<IRedisCacheOptions> options = new();

    [Fact]
    [Trait(nameof(RedisCacheService<Entity<int>, int>), "new()")]
    void Given_A_RedisCacheService_When_Any_Of_The_Parameters_Of_Constructor_Is_Null_An_ArgumentNullException_Should_Be_Thrown()
    {
        // act
        Action actWithLoggerNull = () => new RedisCacheService<Entity<int>, int>(
            logger: null,
            cache: cache.Object,
            options: options.Object);

        Action actWithCacheNull = () => new RedisCacheService<Entity<int>, int>(
            logger: logger.Object,
            cache: null,
            options: options.Object);

        Action actWithOptionsNull = () => new RedisCacheService<Entity<int>, int>(
            logger: logger.Object,
            cache: cache.Object,
            options: null);

        // assert
        actWithLoggerNull
            .Should().Throw<ArgumentNullException>().WithParameterName(nameof(logger));

        actWithCacheNull
            .Should().Throw<ArgumentNullException>().WithParameterName(nameof(cache));

        actWithOptionsNull
            .Should().Throw<ArgumentNullException>().WithParameterName(nameof(options));
    }

    [Fact]
    [Trait(nameof(RedisCacheService<Entity<int>, int>), nameof(IRedisCacheService<Entity<int>, int>.SetAsync))]
    async Task Given_A_New_Entity_When_The_Cache_Is_Disabled_The_Record_Should_Not_Be_Inserted_Into_The_Cache()
    {
        // arrange
        options.Setup(_ => _.Enabled)
            .Returns(false);

        var service = CreateRedisCacheService();

        // act
        await service.SetAsync(default, default, default);

        // assert
        cache
            .Verify(_ => _.SetAsync(It.IsAny<string>(),
                                    It.IsAny<byte[]>(),
                                    It.IsAny<DistributedCacheEntryOptions>(),
                                    It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    [Trait(nameof(RedisCacheService<Entity<int>, int>), nameof(IRedisCacheService<Entity<int>, int>.SetAsync))]
    async Task Given_An_Entity_When_It_Is_Null_No_Value_Should_Be_Inserted_In_The_Cache()
    {
        // arrange
        options.Setup(_ => _.Enabled)
            .Returns(true);

        var service = CreateRedisCacheService();

        // act
        await service.SetAsync(default, default, default);

        // assert
        cache
            .Verify(_ => _.SetAsync(It.IsAny<string>(),
                                    It.IsAny<byte[]>(),
                                    It.IsAny<DistributedCacheEntryOptions>(),
                                    It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    [Trait(nameof(RedisCacheService<Entity<int>, int>), nameof(IRedisCacheService<Entity<int>, int>.SetAsync))]
    async Task Given_An_Entity_When_An_Exception_Occurs_It_Should_Not_Be_Thrown_To_The_Service()
    {
        // arrange
        options.Setup(_ => _.Enabled)
            .Returns(true);

        options.Setup(_ => _.ExpirationTime)
            .Returns(TimeSpan.Parse("00:01:00"));

        cache
            .Setup(_ => _.SetAsync(It.IsAny<string>(),
                                   It.IsAny<byte[]>(),
                                   It.IsAny<DistributedCacheEntryOptions>(),
                                   It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TaskCanceledException());

        var service = CreateRedisCacheService();

        // act
        Func<Task> willNotThrowAnException = () => service.SetAsync(new Team(), "siege:teams:teamliquid", default);

        // assert
        await willNotThrowAnException
            .Should().NotThrowAsync<TaskCanceledException>();

        cache
           .Verify(_ => _.SetAsync(It.Is<string>(_ => _.Equals("siege:teams:teamliquid")),
                                   It.IsAny<byte[]>(),
                                   It.IsAny<DistributedCacheEntryOptions>(),
                                   It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    [Trait(nameof(RedisCacheService<Entity<int>, int>), nameof(IRedisCacheService<Entity<int>, int>.SetAsync))]
    async Task Given_An_Entity_When_Its_Value_Is_Not_Null_And_No_Exception_Occurs_A_New_Record_Must_Be_Inserted_In_The_Cache()
    {
        // arrange
        byte[]? expected = default;

        options.Setup(_ => _.Enabled)
            .Returns(true);

        options.Setup(_ => _.ExpirationTime)
            .Returns(TimeSpan.Parse("00:01:00"));

        cache
            .Setup(_ => _.SetAsync(It.Is<string>(_ => _.Equals("siege:teams:teamliquid")),
                                   It.IsAny<byte[]>(),
                                   It.IsAny<DistributedCacheEntryOptions>(),
                                   It.IsAny<CancellationToken>()))
            .Callback((string key, byte[] content, DistributedCacheEntryOptions cacheoptions, CancellationToken token) =>
            {
                expected = content;
            });

        var service = CreateRedisCacheService();

        // act
        await service.SetAsync(new Team(), "siege:teams:teamliquid", default);

        // assert
        cache
            .Verify(_ => _.SetAsync(It.Is<string>(_ => _.Equals("siege:teams:teamliquid")),
                                    It.Is<byte[]>(_ => _.Equals(expected)),
                                    It.Is<DistributedCacheEntryOptions>(_ => _.AbsoluteExpirationRelativeToNow.Value.Minutes.Equals(1)),
                                    It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    [Trait(nameof(RedisCacheService<Entity<int>, int>), nameof(IRedisCacheService<Entity<int>, int>.GetAsync))]
    async Task Given_A_Cache_Key_When_The_Enabled_Flag_Is_False_The_Cache_Should_Not_Be_Queried()
    {
        // arrange
        options.Setup(_ => _.Enabled)
            .Returns(false);

        var service = CreateRedisCacheService();

        // act
        await service.GetAsync("siege:teams:teamliquid", default);

        // assert
        cache
            .Verify(_ => _.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    [Trait(nameof(RedisCacheService<Entity<int>, int>), nameof(IRedisCacheService<Entity<int>, int>.GetAsync))]
    async Task Given_A_Cache_Key_When_An_Exception_Occurs_It_Should_Not_Be_Thrown_To_The_Service()
    {
        // arrange
        options.Setup(_ => _.Enabled)
            .Returns(true);

        cache
            .Setup(_ => _.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new TaskCanceledException()).Verifiable();

        var service = CreateRedisCacheService();

        // act
        var result = await service.GetAsync("siege:teams:teamliquid", default);

        // assert
        result.Should().BeNull();

        cache
            .Verify(_ => _.GetAsync(It.Is<string>(_ => _.Equals("siege:teams:teamliquid")),
                                    It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    [Trait(nameof(RedisCacheService<Entity<int>, int>), nameof(IRedisCacheService<Entity<int>, int>.GetAsync))]
    async Task Given_A_Cache_Key_When_There_Is_No_Record_With_The_Given_Key_Null_Should_Be_Returned()
    {
        // arrange
        options.Setup(_ => _.Enabled)
            .Returns(true);

        var cached = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(default));

        cache
            .Setup(_ => _.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cached).Verifiable();

        var service = CreateRedisCacheService();

        // act
        var result = await service.GetAsync("siege:teams:teamliquid", default);

        // assert
        result.Should().BeNull();

        cache
            .Verify(_ => _.GetAsync(It.Is<string>(_ => _.Equals("siege:teams:teamliquid")),
                                    It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    [Trait(nameof(RedisCacheService<Entity<int>, int>), nameof(IRedisCacheService<Entity<int>, int>.GetAsync))]
    async Task Given_A_Cache_Key_When_The_Record_Exists_In_The_Cache_Should_Be_Returned()
    {
        // arrange
        options.Setup(_ => _.Enabled)
            .Returns(true);

        var team = new Team
        {
            Id = 1
        };

        var json = JsonConvert.SerializeObject(team, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        });

        var cached = Encoding.UTF8.GetBytes(json);

        cache
            .Setup(_ => _.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cached).Verifiable();

        var service = CreateRedisCacheService();

        // act
        var result = await service.GetAsync("siege:teams:teamliquid", default);

        // assert
        result.Should().NotBeNull();

        result.Id.Should().Be(1);

        cache
            .Verify(_ => _.GetAsync(It.Is<string>(_ => _.Equals("siege:teams:teamliquid")),
                                    It.IsAny<CancellationToken>()), Times.Once);
    }

    private IRedisCacheService<Entity<int>, int> CreateRedisCacheService()
        => new RedisCacheService<Entity<int>, int>(logger.Object, cache.Object, options.Object);
}
