using Veesy.Domain.Constants;
using Veesy.Domain.Exception;

namespace Veesy.Validators;

public class MediaValidators
{
    public ResultDto ValidateMediaExtension(string extension)
    {
        if (MediaCostants.MediaExtension.Contains(extension.ToUpper()))
            return new ResultDto(true, "");
        return new ResultDto(false, "File extension not supported.");
    }

    public ResultDto ValidateSizeUpload(long size, long maxSizePlan)
    {
        if (size > maxSizePlan)
            return new ResultDto(false, "Reached maximum subscription size");
        return new ResultDto(true, "");
    }
}