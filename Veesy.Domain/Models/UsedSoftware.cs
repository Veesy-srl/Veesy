namespace Veesy.Domain.Models;

public class UsedSoftware
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<UsedSoftware> UsedSoftwares { get; set; }
}