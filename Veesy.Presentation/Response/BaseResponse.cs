using System.Text.Json.Serialization;

namespace Veesy.Presentation.Response;

public class BaseResponse
{
    public BaseResponse(int errorCode, string errorMessage)
    {
        Error = new ErrorDto
        {
            Code = errorCode,
            Message = errorMessage
        };
    }
    
    public BaseResponse()
    {
        Error = new ErrorDto
        {
            Code = 0,
            Message = ""
        };
    }
    [JsonPropertyName("error")]
    public ErrorDto Error { get; set; }
}

public class ErrorDto
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
    
}