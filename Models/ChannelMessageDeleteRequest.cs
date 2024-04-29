using System.ComponentModel.DataAnnotations;

namespace Models;

public class ChannelMessageDeleteRequest
{
    [Required] public string ChannelId { get; set; }

    [Required] public string MessageId { get; set; }
}