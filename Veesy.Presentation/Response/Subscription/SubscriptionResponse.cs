using System.Text.Json.Serialization;

namespace Veesy.Presentation.Response.Subscription;

public class SubscriptionResponse : BaseResponse
{
    public SubscriptionResponse(SubscriptionDto data)
    {
        Data = data;
        Error = new ErrorDto();
    }
    public SubscriptionResponse(int code, string message)
    {
        Error = new ErrorDto
        {
            Code = code,
            Message = message
        };
    }

    [JsonPropertyName("data")]
    public SubscriptionDto? Data { get; set; }
}

public class SubscriptionDto
{
    [JsonPropertyName("subscription")]
    public string Subscription { get; set; }
}