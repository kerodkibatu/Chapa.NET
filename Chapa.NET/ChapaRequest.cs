using Newtonsoft.Json;

namespace ChapaNET;

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
