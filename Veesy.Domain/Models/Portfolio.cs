namespace Veesy.Domain.Models;

public class Portfolio : TrackableEntity
{
    public Guid Id { get; set; }
    public string MyUserId { get; set; } 
    public MyUser MyUser { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Note { get; set; }
    public bool IsPublic { get; set; }
    public string Password { get; set; }
    public string Link { get; set; }
    public int Status { get; set; }
    /// <summary>
    /// campo che identifica se è il portfolio principale (quello mostrato nei risultati di ricerca)
    /// </summary>
    public bool IsMain { get; set; }
    public virtual List<PortfolioMedia> PortfolioMedias { get; set; }
    public virtual List<PortfolioSector> PortfolioSectors { get; set; }
}