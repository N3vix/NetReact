using System.ComponentModel.DataAnnotations;

namespace Models;

public class ChannelMessageDeleteRequest
{
    [Required]
    public string MessageId { get; set; }
}
