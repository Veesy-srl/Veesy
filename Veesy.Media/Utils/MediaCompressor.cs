using System.Drawing;
using Format = Veesy.Domain.Models.Format;

namespace Veesy.Media.Utils;

public static class MediaCompressor
{
    public static Stream CompressImage(Stream stream, Format format, Domain.Models.Media OriginalMedia)
    {
      try
      {
            // Convert stream to image
            using var image = Image.FromStream(stream);
            
            //float maxHeight = 720.0f;
            float maxWidth = format.Width;
            int newWidth;
            int newHeight;

            var originalBMP = new Bitmap(image);
            
            if (OriginalMedia.Width > maxWidth)
            {
                // To preserve the aspect ratio  
                float ratioX = (float)format.Width / (float)OriginalMedia.Width;
                //float ratioY = (float)maxHeight / (float)originalHeight;
                //float ratio = Math.Min(ratioX, ratioY);
                newWidth = (int)(OriginalMedia.Width * ratioX);
                newHeight = (int)(OriginalMedia.Height * ratioX);
            }

            else
            {
                //TODO: Bisogna comunicare al chiamante che la risoluzione dell'immagine è già inferiore ala risoluzione di compressione
                return null;
            }

            var bitmap = new Bitmap(originalBMP, newWidth, newHeight);
            return ToMemoryStream(bitmap);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    public static MemoryStream ToMemoryStream(this Bitmap b)
    {
        MemoryStream ms = new MemoryStream();
        b.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        return ms;
    }

    public static Stream CompressVideo(Stream mediaBlobContent, Format format, Domain.Models.Media media)
    {
        var setting = new NReco.VideoConverter.ConvertSettings();
        float maxWidth = format.Width;
        int newWidth = media.Width;
        int newHeight = media.Height;

        if (media.Width > maxWidth)
        {
            // To preserve the aspect ratio  
            float ratioX = (float)format.Width / (float)media.Width;
            //float ratioY = (float)maxHeight / (float)originalHeight;
            //float ratio = Math.Min(ratioX, ratioY);
            newWidth = (int)(media.Width * ratioX);
            newHeight = (int)(media.Height * ratioX);
        }
        setting.SetVideoFrameSize(newWidth, newHeight);
        setting.VideoCodec = "h264";
        var converter = new NReco.VideoConverter.FFMpegConverter();
        Stream processedVideo = new System.IO.MemoryStream();
        converter.ConvertLiveMedia(mediaBlobContent, media.Type, processedVideo, "mp4", setting);
        return processedVideo;
    }
}