using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Veesy.Media.Utils;

public static class MediaCompressor
{
    public static Stream CompressImage(Stream stream)
    {
      try
      {
            // Convert stream to image
            using var image = Image.FromStream(stream);
            
            float maxHeight = 720.0f;
            float maxWidth = 1280.0f;
            int newWidth;
            int newHeight;

            var originalBMP = new Bitmap(image);
            int originalWidth = originalBMP.Width;
            int originalHeight = originalBMP.Height;
            
            if (originalWidth > maxWidth || originalHeight > maxHeight)
            {
                // To preserve the aspect ratio  
                float ratioX = (float)maxWidth / (float)originalWidth;
                float ratioY = (float)maxHeight / (float)originalHeight;
                float ratio = Math.Min(ratioX, ratioY);
                newWidth = (int)(originalWidth * ratio);
                newHeight = (int)(originalHeight * ratio);
            }

            else
            {
                //TODO: Bisogna comunicare al chiamante che la risoluzione dell'immagine è già inferiore ala risoluzione di compressione
                return null;
            }

            var bitmap = new Bitmap(originalBMP, newWidth, newHeight);
            return ToMemoryStream(bitmap);
            /*var imgGraph = Graphics.FromImage(bitmap);

            imgGraph.SmoothingMode = SmoothingMode.Default;
            imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);

            
            var extension = Path.GetExtension(targetPath).ToLower();
            // for file extension having png and gif
            if (extension == ".png" || extension == ".gif")
            {
                // Save image to targetPath
                bitmap.Save(output, image.);
            }

            // for file extension having .jpg or .jpeg
            else if (extension == ".jpg" || extension == ".jpeg")
            {
                ImageCodecInfo jpgEncoder = MediaInfo.GetEncoder(ImageFormat.Jpeg);
                Encoder myEncoder = Encoder.Quality;
                var encoderParameters = new EncoderParameters(1);
                var parameter = new EncoderParameter(myEncoder, 50L);
                encoderParameters.Param[0] = parameter;

                // Save image to targetPath
                bitmap.Save(output, jpgEncoder, encoderParameters);
            }
            bitmap.Dispose();
            imgGraph.Dispose();
            originalBMP.Dispose();*/
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
    
}