namespace Veesy.Domain.Constants;

public static class MediaCostants
{
    public static string[] imageExtensions =
    {
        ".PNG", ".JPG", ".JPEG", ".BMP", ".GIF"
    };
    
    public static class BlobMediaSections
    {
        public const string OriginalMedia = "OriginalMedia";
        public static string CompressedMedia = "CompressedMedia";
        public static string ProfileMedia = "ProfileImage";
    }
    
    public static string[] videoExtensions =
    {
        ".AVI", ".MP4", ".DIVX", ".WMV"
    };

    public static string[] mediaExtensions => videoExtensions.Concat(imageExtensions).ToArray();
}
