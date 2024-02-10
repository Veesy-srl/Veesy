namespace Veesy.Domain.Models;

/// <summary>
/// Tabella per il salvataggio dei media originali caricati dall'utente.
/// <param name="Id">Primary Key.</param>
/// <typeparam name="Id">Guid</typeparam>
/// <param name="FileName">Nome originale del file caricat.</param>
/// <typeparam name="FileName">string</typeparam>
/// <param name="ProcessedName">Nome del file processato nel caso in cui serva attribuire un nome crittografato per il salvataggio cloud.</param>
/// <typeparam name="ProcessedName">string</typeparam>
/// <param name="Type">Formato dell'immagine caricata.</param>
/// <typeparam name="Type">string</typeparam>
/// <param name="UploadDate">Data UTC nella quale è stata caricata l'immagine.</param>
/// <typeparam name="UploadDate">DateTime</typeparam>
/// <param name="Path">URL per il recupero dell'immagine.</param>
/// <typeparam name="Path">string</typeparam>
/// <param name="Quality">Qualità dell'immagine originale.</param>
/// <typeparam name="Quality">string</typeparam>
/// </summary>
public class Media : TrackableEntity
{
    public Guid Id { get; set; }
    public string MyUserId { get; set; }
    public MyUser MyUser { get; set; }
    public string FileName { get; set; }
    public string OriginalFileName { get; set; }
    public string Type { get; set; }
    public DateTime UploadDate { get; set; }
    public string? Credits { get; set; }
    // public int Status { get; set; }
    public long Size { get; set; }
    
    public Guid? NestedPortfolioLinks { get; set; }
    // public int Width { get; set; }
    // public int Height { get; set; }
    public virtual List<MediaCategory> MediaCategories { get; set; }
    public virtual List<MediaTag> MediaTags { get; set; }
    public virtual List<PortfolioMedia> PortfolioMedias { get; set; }
}