using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class ServerDetails
{
    [Column(TypeName = "nvarchar(40)", Order = 1)]
    public string Id { get; set; }

    [Column(TypeName = "nvarchar(100)", Order = 2)]
    public string Name { get; set; }

    [Column(TypeName = "nvarchar(400)", Order = 3)]
    public string? Description { get; set; }
}