using System;

namespace Veesy.Domain.Models;

public class PortfolioMedia : TrackableEntity
{
    public Guid MediaId { get; set; }
    public Media Media { get; set; }
    public Guid PortfolioId { get; set; }
    public Portfolio Portfolio { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
}