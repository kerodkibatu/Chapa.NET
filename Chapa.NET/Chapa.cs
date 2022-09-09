using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace ChapaNET;
public enum Validity { Valid, Invalid }
public class Chapa
{

    ChapaConfig Config { get; set; }
    RestClient Client;
    public Chapa(string SECRET_KEY)
    {
        Config = new() { API_SECRET = SECRET_KEY };
        Client = new RestClient(ChapaConfig.BASE_PATH)
            .AddDefaultHeader(ChapaConfig.AUTH_HEADER, "Bearer " + Config.API_SECRET)
            .UseJson()
            .UseNewtonsoftJson();
    }
    public Chapa(ChapaConfig config) : this(config.API_SECRET) { }
    public async Task<ChapaResponse> Request(ChapaRequest request)
    {
        var response = (await Client.PostJsonAsync<ChapaRequest, ChapaResponse>("/transaction/initialize", request))!;
        return response;
    }
    public Validity Verify(string TransactionReference)
    {
        return Client.Get(new($"/transaction/verify/{TransactionReference}")).StatusCode == System.Net.HttpStatusCode.OK ? Validity.Valid : Validity.Invalid;
    }
}
public class ChapaRequest
{
    [JsonProperty("amount")]
    [JsonRequired]
    public double Amount { get; set; }
    [JsonProperty("currency")]
    [JsonRequired]
    public string Currency { get; set; }
    [JsonProperty("email")]
    [JsonRequired]
    public string Email { get; set; }
    [JsonProperty("first_name")]
    [JsonRequired]
    public string FirstName { get; set; }
    [JsonProperty("last_name")]
    [JsonRequired]
    public string LastName { get; set; }
    [JsonProperty("tx_ref")]
    [JsonRequired]
    public string TransactionReference { get; set; }
    [JsonProperty("callback_url")]
    public string? CallbackUrl { get; set; }
    [JsonProperty("return_url")]
    public string? ReturnUrl { get; set; }

    [JsonProperty("customization[title]")]
    public string? CustomTitle { get; set; }
    [JsonProperty("customization[description]")]
    public string? CustomDescription { get; set; }
    [JsonProperty("customization[logo]")]
    public string? CustomLogo { get; set; }
    [JsonConstructor]

    public ChapaRequest(double amount, string email
        , string first_name, string last_name
        , string tx_ref
        , string currency = "ETB"
        , string? callback_url = null
        , string? return_url = null
        , [JsonProperty("customization[title]")] string? customTitle = null
        , [JsonProperty("customization[description]")] string? customDescription = null
        , [JsonProperty("customization[logo]")] string? customLogo = null)
    {
        Amount = amount;
        Currency = currency;
        Email = email;
        FirstName = first_name;
        LastName = last_name;
        TransactionReference = tx_ref;
        CallbackUrl = callback_url;
        ReturnUrl = return_url;
        CustomTitle = customTitle;
        CustomDescription = customDescription;
        CustomLogo = customLogo;
    }
}


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
