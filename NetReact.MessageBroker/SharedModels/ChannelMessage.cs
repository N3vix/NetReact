namespace NetReact.MessageBroker.SharedModels;

public class ChannelMessageCreated
{
    public string SenderId { get; set; }
    public string ChannelId { get; set; }
    public string Content { get; set; }
    public byte[]? Image { get; set; }
}