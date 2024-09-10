using System.Text.Json.Serialization;

namespace Veesy.Presentation.Response.Mail;

public class MailResponse : BaseResponse
{
    public MailResponse(MailDto data)
    {
        Data = data;
        Error = new ErrorDto();
    }
    public MailResponse(int code, string message)
    {
        Error = new ErrorDto
        {
            Code = code,
            Message = message
        };
    }

    [JsonPropertyName("data")]
    public MailDto? Data { get; set; }
}

public class MailDto
{
    [JsonPropertyName("mail-address")]
    public string MailAddress { get; set; }
}