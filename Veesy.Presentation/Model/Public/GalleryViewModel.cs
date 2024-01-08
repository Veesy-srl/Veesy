using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Public;

public class GalleryViewModel
{
    public List<MediaDto> MediaDtos { get; set; }
    public string BasePathImages { get; set; }
    public string BasePathAzure { get; set; }
}