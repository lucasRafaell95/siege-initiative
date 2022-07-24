using FluentAssertions;
using SiegeInitiative.DataContracts.OperationResult;
using SiegeInitiative.DataContracts.OperationResult.Base;
using System;
using Xunit;

namespace SiegeInitiative.DataContracts.Test.OperationResult;

public sealed class MessageTest
{
    [Fact]
    [Trait(nameof(Message), nameof(Message.Create))]
    public void Given_The_Creation_Of_A_Message_When_A_Text_Is_Informed_An_ArgumentNullException_Must_Be_Thrown()
    {
        // act
        Func<Message> shouldThrowException = () => Message.Create(default, default);

        // assert
        shouldThrowException
            .Should().Throw<ArgumentNullException>();
    }

    [Fact]
    [Trait(nameof(Message), nameof(Message.Create))]
    public void Given_A_Message_When_Its_Type_Is_Not_Informed_It_Must_Be_Of_Type_BusinessError()
    {
        // arrange
        var property = "Nickname";
        var text = "Invalid player nickname";

        // act
        var message = Message.Create(text, property);

        // assert
        message.Should().NotBeNull();
        message.Text.Should().Be(text);
        message.Property.Should().Be(property);
        message.MessageType.Should().Be(MessageType.BusinessError);
    }
}