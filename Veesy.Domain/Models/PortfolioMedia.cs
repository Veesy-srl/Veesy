namespace Veesy.Domain.Models;

public class PortfolioMedia
{
    public Guid MediaFormatId { get; set; }
    public Guid PortfolioId { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
}