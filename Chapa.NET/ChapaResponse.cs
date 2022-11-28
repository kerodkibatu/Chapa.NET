using Newtonsoft.Json;

namespace ChapaNET;

public class ChapaResponse
{
    [JsonProperty("message")]
    public string? Message { get; }
    [JsonProperty("status")]
    public string? Status { get; }
    [JsonProperty("data")]
    ResponseUrls? Urls { get; }

    public string? CheckoutUrl => Urls?.CheckoutUrl;
    public override string ToString() => JsonConvert.SerializeObject(this);
    
    class ResponseUrls
    {
        [JsonProperty("checkout_url")]
        public string? CheckoutUrl { get; set; }
    }
}