﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProjectNumber { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int CustomerId { get; set; }
    public CustomerEntity Customer { get; set; } = null!;
    public int StatusId { get; set; }
    public StatusTypeEntity Status { get; set; } = null!;
    public int? UserId { get; set; }
    public UserEntity? User { get; set; } = null!;
    public int? ProductId { get; set; }
    public ProductEntity Product { get; set; } = null!;
    public decimal PricePerHour { get; set; }
    public int TotalHours { get; set; }
}
