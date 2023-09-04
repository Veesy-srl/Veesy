namespace Veesy.Media.Constants;

public static class MediaCostants
{
    public static string[] imageExtensions =
    {
        ".PNG", ".JPG", ".JPEG", ".BMP", ".GIF"
    };
    
    public static string[] videoExtensions =
    {
        ".AVI", ".MP4", ".DIVX", ".WMV"
    };

    public const string BlobProfileImageDirectory = "ProfileImage";

    public static string[] mediaExtensions => videoExtensions.Concat(imageExtensions).ToArray();
}
