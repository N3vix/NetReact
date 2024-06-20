namespace Models;

public class ChannelDetails
{
    public string Id { get; set; }
    public string ServerId { get; set; }
    public string Name { get; set; }
    public ChannelType Type { get; set; }
}

public enum ChannelType
{
    Text = 0,
    Voice = 1
}


public class ChannelMessage
{
    public string Id { get; set; }
    public string ChannelId { get; set; }
    public string SenderId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Content { get; set; }
    public string Image { get; set; }
    public DateTime? EditedTimestamp { get; set; }
}