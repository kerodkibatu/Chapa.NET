using Newtonsoft.Json;

namespace ChapaNET;

public class ChapaResponse
{
    public string? Message { get; set; }
    public string? Status { get; set; }
    public string? CheckoutUrl { get; set; }
    public override string ToString() => JsonConvert.SerializeObject(this);
}