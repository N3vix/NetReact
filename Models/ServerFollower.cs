using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class ServerFollower
{
    [Column(TypeName = "nvarchar(40)", Order = 1)]
    public string Id { get; set; }

    [Column(TypeName = "nvarchar(40)", Order = 2)]
    public string ServerId { get; set; }

    [Column(TypeName = "nvarchar(40)", Order = 3)]
    public string UserId { get; set; }
}