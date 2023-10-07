using Veesy.Domain.Constants;
using Veesy.Domain.Exceptions;

namespace Veesy.Validators;

public class MediaValidators
{
    public ResultDto ValidateMediaExtension(string extension)
    {
        if (MediaCostants.MediaExtension.Contains(extension.ToUpper()))
            return new ResultDto(true, "");
        return new ResultDto(false, "File extension not supported.");
    }

    public ResultDto ValidateSizeUpload(long size, long sizeTotal, long maxSizePlan, string extension)
    {
        var limit = MediaCostants.MaxSizeSingleMedia[MediaCostants.GetType(extension)];
        if(size > limit)
            return new ResultDto(false, $"Limit size: {(limit / (1024*1024)).ToString("0.")}Mb");
        if (sizeTotal > maxSizePlan)
            return new ResultDto(false, "Reached maximum subscription size");
        return new ResultDto(true, "");
    }
}