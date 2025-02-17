using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Dtos;

public class ProjectDataTransfer
{
    public string ProjectNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public decimal Price { get; set; }
    public int StatusTypeId { get; set; }
    public string StatusTypeName { get; set; }

}
