using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Models;

public class ChannelMessageAddRequest
{
    [Required]
    public string ChannelId { get; set; }

    [Required]
    public string Content { get; set; }
}
