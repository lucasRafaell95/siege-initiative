using SiegeInitiative.DataContracts.OperationResult.Base;

namespace SiegeInitiative.DataContracts.OperationResult;

public sealed class SuccessResult<TResult> : Result<TResult>
{
    public SuccessResult(TResult data) => Data = data;
}