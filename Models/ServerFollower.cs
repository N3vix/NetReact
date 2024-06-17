namespace Models;

public class ServerFollower
{
    public string ServerId { get; set; }
    public ServerDetails Server { get; set; }

    public string UserId { get; set; }
    
    public DateTime FollowDate { get; set; }
}