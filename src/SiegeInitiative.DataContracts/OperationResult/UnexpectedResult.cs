using SiegeInitiative.DataContracts.OperationResult.Base;

namespace SiegeInitiative.DataContracts.OperationResult;

public sealed class UnexpectedResult<TResult> : Result<TResult>
{
    public UnexpectedResult(string error = "There was an unexpected problem")
    {
        Data = default;
        ResultType = ResultType.Unexpected;
        Errors = new List<string> { error };
    }
}