namespace Veesy.Domain.Models;

public class PortfolioMedia : TrackableEntity
{
    public Guid MediaFormatId { get; set; }
    public MediaFormat MediaFormat { get; set; }
    public Guid PortfolioId { get; set; }
    public Portfolio Portfolio { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
}