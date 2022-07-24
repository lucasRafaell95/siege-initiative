using FluentAssertions;
using SiegeInitiative.DataContracts.OperationResult;
using SiegeInitiative.DataContracts.OperationResult.Base;
using System;
using Xunit;

namespace SiegeInitiative.DataContracts.Test.OperationResult.Base;

public sealed class BaseResultTest
{
    [Fact]
    [Trait(nameof(Result), nameof(Result.AddMessage))]
    public void Given_A_BaseResult_When_The_Message_Informed_Is_Null_An_ArgumentNullException_Should_Be_Thrown()
    {
        // arrange
        var result = Result.Create();

        // act
        Func<Result> shouldThrowAgumentNullException = () => result.AddMessage(default);

        // assert
        shouldThrowAgumentNullException
            .Should().Throw<ArgumentNullException>();
    }

    [Fact]
    [Trait(nameof(Result), nameof(Result.AddMessage))]
    public void Given_A_BaseResult_When_A_Message_Is_Not_Informed_The_Result_Must_Not_Have_Errors()
    {
        // act
        var result = Result.Create();

        // assert
        result.Should().NotBeNull();
        result.HasError.Should().BeFalse();
        result.Messages.Count.Should().Be(0);
    }

    [Fact]
    [Trait(nameof(Result), nameof(Result.AddMessage))]
    public void Given_A_BaseResult_When_There_Is_A_Message_It_Must_Be_Added_To_The_Result()
    {
        // arrange
        var message = Message.Create("Invalid Nickname", "Nickname");

        var baseResult = Result.Create();

        // act
        var result = baseResult.AddMessage(message);

        // assert
        result.Should().NotBeNull();
        result.HasError.Should().BeTrue();
        result.Messages.Count.Should().Be(1);
    }
}