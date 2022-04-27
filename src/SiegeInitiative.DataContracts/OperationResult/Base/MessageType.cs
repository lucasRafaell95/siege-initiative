namespace SiegeInitiative.DataContracts.OperationResult.Base;

public enum MessageType
{
    None = 0,
    Success = 1,
    Alert = 2,
    Information = 3,
    BusinessError = 4,
    CriticalError = 5
}