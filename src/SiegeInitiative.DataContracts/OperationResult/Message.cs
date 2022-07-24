using SiegeInitiative.DataContracts.OperationResult.Base;

namespace SiegeInitiative.DataContracts.OperationResult;

public sealed class Message
{
    #region Fields
    public string Text { get; set; }
    public string Property { get; set; }
    public MessageType MessageType { get; set; }

    #endregion

    #region Constructor

    private Message(string text, MessageType messageType = MessageType.BusinessError, string property = null)
    {
        Text = text;
        MessageType = messageType;
        Property = property;
    }

    #endregion

    #region Methods

    public static Message Create(string text, string property, MessageType messageType = MessageType.BusinessError)
    {
        ArgumentNullException.ThrowIfNull(text, nameof(text));

        return new Message(text, messageType, property);
    }

    #endregion
}