namespace Veesy.Service.Dtos;

public class PortfolioMediaDto
{
    public Guid MediaFormatId { get; set; }
    public MediaDto MediaFormat { get; set; }
    public Guid PortfolioId { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
}