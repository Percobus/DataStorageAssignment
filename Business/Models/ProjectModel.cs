namespace Business.Models;

public class ProjectModel
{
    public int ProjectNumber { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public int StatusTypeId { get; set; }
    public string StatusTypeName { get; set; } = null!;
    public decimal Price { get; set; }
    public decimal PricePerHour { get; set; }
    public int TotalHours { get; set; }

    public decimal TotalPrice => Price * TotalHours;
}
