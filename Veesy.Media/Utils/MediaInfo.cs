using System.Drawing.Imaging;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.StaticFiles;

namespace Veesy.Media.Utils;

public static class MediaInfo
{
    private const string UploadsSubDirectory = "FilesUploaded";
    private static readonly FileExtensionContentTypeProvider Provider = new FileExtensionContentTypeProvider();

    public static ImageCodecInfo GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return null;
    }

    public static string GetBoundary(MediaTypeHeaderValue contentType)
    {
        var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

        if (string.IsNullOrWhiteSpace(boundary))
        {
            throw new InvalidDataException("Missing content-type boundary.");
        }

        return boundary;
    }
    
    public static string GetFullFilePath(FileMultipartSection fileSection)
    {
        return !string.IsNullOrEmpty(fileSection.FileName)
            ? Path.Combine(Directory.GetCurrentDirectory(), UploadsSubDirectory, fileSection.FileName)
            : string.Empty;
    }
    
    
    public static string ConvertSizeToString(long bytes)
    {
        var fileSize = new decimal(bytes);
        var kilobyte = new decimal(1024);
        var megabyte = new decimal(1024 * 1024);
        var gigabyte = new decimal(1024 * 1024 * 1024);

        return fileSize switch
        {
            _ when fileSize < kilobyte => "Less then 1KB",
            _ when fileSize < megabyte =>
                $"{Math.Round(fileSize / kilobyte, fileSize < 10 * kilobyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}KB",
            _ when fileSize < gigabyte =>
                $"{Math.Round(fileSize / megabyte, fileSize < 10 * megabyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}MB",
            _ when fileSize >= gigabyte =>
                $"{Math.Round(fileSize / gigabyte, fileSize < 10 * gigabyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}GB",
            _ => "n/a"
        };
    }

    public static (int Width, int Height) GetImageSizeFromStream(Stream stream)
    {
        System.Drawing.Image image = System.Drawing.Image.FromStream (stream);
        if (image != null)
            return (image.Width, image.Height);
        return (0, 0);
    }

    public static (int Width, int Height) GetVideoSizeFromStream(Stream stream)
    {
        return (2160, 2700);
    }
    
    public static string GetContentType(this string fileName)
    {
        if (!Provider.TryGetContentType(fileName, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        return contentType;
    }
}