namespace Veesy.Presentation.Model.Cloud;

public class AboutMediaViewModel
{
    public List<string> MediaList { get; set; }
    public List<string> MediaUser { get; set; }
    public List<string> Usernames { get; set; }
    
    public List<string> Id { get; set; }
    public string BasePath { get; set; }
    public string BasePathImages { get; set; }
    
    public string ApplicationUrl { get; set; }
    
    public List<Guid> PortfolioId { get; set; }
}