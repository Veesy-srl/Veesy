namespace Veesy.Domain.Models;

public class MediaFormat
{
    public Guid Id { get; set; }
    public Guid MediaId { get; set; }
    public Media Media { get; set; }
    public Guid FormatId { get; set; }
    public Format Format { get; set; }
    public string FileName { get; set; }
    public long Size { get; set; }
    public virtual List<PortfolioMedia> PortfolioMedias { get; set; }
}