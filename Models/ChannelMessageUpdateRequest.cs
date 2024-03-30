using System.ComponentModel.DataAnnotations;

namespace Models;

public class ChannelMessageUpdateRequest
{
    [Required]
    public string MessageId { get; set; }

    [Required]
    public string Content { get; set; }
}
