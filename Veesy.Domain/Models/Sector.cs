namespace Veesy.Domain.Models;

/// <summary>
/// Settore nella quale lavora l'utente. Ad esempio Metaverso, CGI, Automotive etc.
/// </summary>
public class Sector
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual List<MyUserSector> MyUserSectors { get; set; }
    public virtual List<PortfolioSector> PortfolioSectors { get; set; }
}