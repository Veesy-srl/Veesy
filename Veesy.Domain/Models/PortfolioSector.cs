namespace Veesy.Domain.Models;

public class PortfolioSector
{
    public Guid SectorId { get; set; }
    public Guid PorfolioId { get; set; }
    public Portfolio Portfolio { get; set; }
    public Sector Sector { get; set; }
    public string Note { get; set; }
    public bool IsPrincipal { get; set; }
}