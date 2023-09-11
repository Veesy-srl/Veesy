namespace Veesy.Domain.Models;

public class Format
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Status { get; set; }
    public string Type { get; set; }
    public virtual List<MediaFormat> MediaFormats { get; set; }
    
}