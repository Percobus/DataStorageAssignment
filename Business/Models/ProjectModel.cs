
namespace Business.Models;

public class ProjectModel
{
    public string ProjectNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public int StatusTypeId { get; set; }
    public string StatusTypeName { get; set; } = null!;
    public decimal Price { get; set; }
    public int EstimatedHours { get; set; }

    public decimal TotalPrice => Price * EstimatedHours;
}
