using System.ComponentModel.DataAnnotations;

namespace Models;

public class ChannelMessageRequest
{
    [Required]
    public string ChannelId { get; set; }

    [Required]
    public string Content { get; set; }
}
