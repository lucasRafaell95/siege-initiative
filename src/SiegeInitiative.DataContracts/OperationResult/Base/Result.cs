namespace SiegeInitiative.DataContracts.OperationResult.Base;

public class Result<TResult>
{
    public TResult Data { get; set; }
    public bool Sucess { get => Errors.Any(); }
    public ResultType ResultType { get; set; } = ResultType.Success;
    public List<string> Errors { get; set; } = new List<string>();
}