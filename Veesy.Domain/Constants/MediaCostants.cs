namespace Veesy.Domain.Constants;

public static class MediaCostants
{
    public static string[] ImageExtensions =
    {
        ".PNG", ".JPG", ".JPEG", ".GIF"
    };

    public static string[] VideoExtensions =
    {
        ".MP4", ".MOV"
    };

    public static string[] GetType(string extension)
    {
        if (ImageExtensions.Contains(extension.ToUpper()))
        {
            return ImageExtensions;
        }
        else if(VideoExtensions.Contains(extension.ToUpper()))
        {
            return VideoExtensions;
        }

        return null;
    }

    public static string[] MediaExtension => VideoExtensions.Concat(ImageExtensions).ToArray();

    public static Dictionary<string[], long> MaxSizeSingleMedia = new Dictionary<string[], long>
    {
        {ImageExtensions,36700160}, {VideoExtensions, 304087040}    
    };
    
    public static class BlobMediaSections
    {
        public const string OriginalMedia = "OriginalMedia";
        public static string CompressedMedia = "CompressedMedia";
        public static string ProfileMedia = "ProfileImage";
    }
}
