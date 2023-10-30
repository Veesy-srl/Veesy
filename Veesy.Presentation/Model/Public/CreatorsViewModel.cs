using Veesy.Domain.Models;

public class CreatorsViewModel
{
    public List<MyUser> User { get; set; }
    public List<string> CategoryWorks { get; set; }
    public string BasePathImages { get; set; }
    
    public string ApplicationUrl { get; set; }
    
    public List<Guid> PortfolioId { get; set; }
}