namespace Veesy.Domain.Models;

public class MediaFormat
{
    public Guid Id { get; set; }
    public Guid MediaId { get; set; }
    public Media Media { get; set; }
    public Guid FormatId { get; set; }
    public Format Format { get; set; }
    public string ProcessedFileName { get; set; }
    public string Path { get; set; }
    public List<PortfolioMedia> PortfolioMedias { get; set; }
}