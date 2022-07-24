using SiegeInitiative.DataContracts.OperationResult.Base;
using System.Runtime.Serialization;

namespace SiegeInitiative.DataContracts.OperationResult;

[DataContract]
public class Result<TResult> : Result
{
    #region Fields

    public TResult Data { get; set; }

    #endregion

    #region Constructors

    private Result() { }

    private Result(TResult data) => SetResult(data);

    private Result(Message message) : base(message) { }

    private Result(TResult data, Message message) : base(message)
        => SetResult(data);

    #endregion

    #region Methods

    public static Result<TResult> CreateResult() => new();

    public static Result<TResult> CreateResult(TResult data) => new(data);

    public static Result<TResult> CreateResult(Message message) => new(message);

    public static Result<TResult> CreateResult(TResult data, Message message) => new(data, message);

    public Result<TResult> SetResult(TResult data)
    {
        Data = data;

        return this;
    }

    #endregion
}