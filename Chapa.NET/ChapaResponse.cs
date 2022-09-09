using Newtonsoft.Json;

namespace ChapaNET;

public class ChapaResponse
{
    [JsonProperty("message")]
    public string? Message { get; set; }
    [JsonProperty("status")]
    public string? Status { get; set; }
    [JsonProperty("data")]
    public ResponseUrls? Urls { get; set; }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}

public class ResponseUrls
{
    [JsonProperty("checkout_url")]
    public string? CheckoutUrl { get; set; }
}
