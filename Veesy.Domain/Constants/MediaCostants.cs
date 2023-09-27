namespace Veesy.Domain.Constants;

public static class MediaCostants
{
    public static string[] ImageExtensions =
    {
        ".PNG", ".JPG", ".JPEG", ".GIF"
    };
    
    public static string[] VideoExtensions =
    {
        ".AVI", ".MP4", ".MOV"
    };
    
    public static string[] MediaExtension => VideoExtensions.Concat(ImageExtensions).ToArray();
    
    public static class BlobMediaSections
    {
        public const string OriginalMedia = "OriginalMedia";
        public static string CompressedMedia = "CompressedMedia";
        public static string ProfileMedia = "ProfileImage";
    }
}
