using System.ComponentModel.DataAnnotations;

namespace Models;

public class ServerAddRequest
{
    [Required] public string Name { get; init; }
}