namespace NetReact.MessageBroker.SharedModels;

public class CreateChannelMessageCommand
{
    public string SenderId { get; set; }
    public string ChannelId { get; set; }
    public string Content { get; set; }
    public string Image { get; set; }
}