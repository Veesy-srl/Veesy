namespace Veesy.Domain.Models;

public class TmpMedia
{
    public Guid Id { get; set; }
    public Guid MediaId { get; set; }
    public Media Media { get; set; }
    public string FileName { get; set; }
    public string Type { get; set; }
    public byte[] MediaBase64 { get; set; }
    
}