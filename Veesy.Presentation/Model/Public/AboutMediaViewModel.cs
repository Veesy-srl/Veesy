using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Cloud;

public class AboutMediaViewModel
{
    public List<Domain.Models.Media> MediaList { get; set; }
    public List<string> MediaUser { get; set; }
    public List<string> Usernames { get; set; }
    public string BasePath { get; set; }
    
    public string BasePathImages { get; set; }
}