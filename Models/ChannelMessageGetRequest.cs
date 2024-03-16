using System.ComponentModel.DataAnnotations;

namespace Models;

public class ChannelMessageGetRequest
{
    [Required]
    public string ChannelId { get; set; }

    [Required]
    public int Take { get; set; }

    public DateTime DateTime { get; set; }
}
