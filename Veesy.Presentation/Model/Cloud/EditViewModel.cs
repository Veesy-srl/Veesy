using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Cloud;

public class EditViewModel
{
    public MediaDto Media { get; set; }
    public MediaDto PreviousMedia { get; set; }
    public MediaDto NextMedia { get; set; }
    public string BasePath { get; set; }
    public string? Username { get; set; }
}