using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Models;

public class ChannelMessageAddRequest
{
    [Required]
    public string ChannelId { get; set; }

    [Required]
    public string Content { get; set; }

    public IFormFile Image { get; set; }
}
