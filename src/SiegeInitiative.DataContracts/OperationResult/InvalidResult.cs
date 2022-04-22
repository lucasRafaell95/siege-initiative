using SiegeInitiative.DataContracts.OperationResult.Base;

namespace SiegeInitiative.DataContracts.OperationResult;

public sealed class InvalidResult<TResult> : Result<TResult>
{
    public InvalidResult(string error = "The input was invalid.")
    {
        Data = default;
        ResultType = ResultType.Invalid;
        Errors = new List<string> { error };
    }
}