using Veesy.Domain.Models;

namespace Veesy.Presentation.Model.Public;

public class CreatorsViewModel
{
    public List<MyUser> User { get; set; }
    public List<string> CategoryWorks { get; set; }
    public string BasePathImages { get; set; }
    
    public string ApplicationUrl { get; set; }
    
    public List<Guid> PortfolioId { get; set; }
    public List<Domain.Models.Portfolio> Portfolios { get; set; }
}