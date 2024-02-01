using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Public;

public class GalleryViewModel
{
    public List<MediaGalleryDto> MediaGalleryDtos{ get; set; }
    public string BasePathImages { get; set; }
    public string BasePathAzure { get; set; }
    
    public string BasePathCode { get; set; }
}