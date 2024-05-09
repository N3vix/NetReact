namespace NetReact.MessageBroker.SharedModels;

public class EditChannelMessageCommand
{
    public string MessageId { get; set; }
    public string NewContent { get; set; }
}