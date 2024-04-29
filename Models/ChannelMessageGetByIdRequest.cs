using System.ComponentModel.DataAnnotations;

namespace Models;

public class ChannelMessageGetByIdRequest
{
    [Required] public string ChannelId { get; set; }

    [Required] public string MessageId { get; set; }
}