namespace Veesy.Domain.Exception;

public class ResultDto
{
    public readonly bool Success;
    public readonly string Message;
    public readonly int? Status;

    public ResultDto(bool success, string message, int? status = null)
    {
        Success = success;
        Message = message;
        Status = status;
    }
}