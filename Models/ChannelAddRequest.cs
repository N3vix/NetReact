using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models;

public class ChannelAddRequest
{
    [Required] public string ServerId { get; init; }

    [Required] public string Name { get; init; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ChannelType Type { get; init; }
}