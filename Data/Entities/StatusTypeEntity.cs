using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class StatusTypeEntity
{
    [Key]
    public int Id { get; set; }
    public string StatusName { get; set; } = null!;
    public bool IsActive { get; set; }
}

