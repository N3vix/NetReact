namespace Models;

public class ChannelDetails
{
    public string Id { get; set; }
    public string Name { get; set; }
    public ChannelType Type { get; set; }

    public string ServerId { get; set; }
}

public enum ChannelType
{
    Text,
    Voice
}
