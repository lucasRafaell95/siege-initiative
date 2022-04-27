namespace SiegeInitiative.DataContracts.OperationResult.Base;

public class Result
{
    #region Fields

    private readonly List<Message> messages = new List<Message>();

    public IReadOnlyCollection<Message> Messages => messages;

    public bool HasError => ContainsError();

    #endregion

    #region Constructor

    internal protected Result() { }

    internal Result(Message message) => AddMessage(message);

    #endregion

    #region Methods

    public static Result Create()
        => new();

    public static Result Ok()
        => new();

    public static Result Ok(Message message)
        => new(message);

    public Result AddMessage(Message message)
    {
        ArgumentNullException.ThrowIfNull(message, nameof(message));

        messages.Add(message);

        return this;
    }

    #endregion

    #region Private metods

    private bool ContainsError()
        => messages.Any(_ => _.MessageType.Equals(MessageType.BusinessError) ||
                             _.MessageType.Equals(MessageType.CriticalError));

    #endregion
}