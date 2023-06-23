namespace Veesy.Domain.Models;

public class MediaTag
{
    public Guid Id { get; set; }
    public Guid MediaId { get; set; }
    public Media Media { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}