using SiegeInitiative.DataContracts.OperationResult;
using SiegeInitiative.DataContracts.OperationResult.Base;

namespace SiegeInitiative.DataContracts.Extension;

public static class ResultExtension
{
    public static Result WithBusinessError(this Result result, string text, string property = "")
    {
        var message = Message.Create(text, property, messageType: MessageType.BusinessError);

        return result.AddMessage(message);
    }

    public static Result<TResult> WithBusinessError<TResult>(this Result<TResult> result, string text, string property = "")
    {
        var message = Message.Create(text, property, MessageType.BusinessError);

        result.AddMessage(message);

        return result;
    }

    public static Result WithCriticalError(this Result result, string text, string property = "")
    {
        var message = Message.Create(text, property, MessageType.CriticalError);

        result.AddMessage(message);

        return result;
    }

    public static Result<TResult> WithCriticalError<TResult>(this Result<TResult> result, string text, string property = "")
    {
        var message = Message.Create(text, property, MessageType.CriticalError);

        result.AddMessage(message);

        return result;
    }
}