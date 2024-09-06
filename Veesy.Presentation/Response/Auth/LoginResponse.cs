using System.Text.Json.Serialization;
using Veesy.Service.Dtos;

namespace Veesy.Presentation.Response.Auth;

public class LoginResponse : BaseResponse
{
    public LoginResponse(LoginDto data)
    {
        Data = data;
        Error = new ErrorDto();
    }
    public LoginResponse(int code, string message)
    {
        Error = new ErrorDto
        {
            Code = code,
            Message = message
        };
    }

    [JsonPropertyName("data")]
    public LoginDto? Data { get; set; }
}

public class LoginDto
{
    
    [JsonPropertyName("token")]
    public string Token { get; set; }
    
    [JsonPropertyName("user")]
    public UserInfoDto User { get; set; }
}