namespace Veesy.Domain.Models;

public class Category : TrackableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual List<MediaCategory> MediaCategories { get; set; }
}