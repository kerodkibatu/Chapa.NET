using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ChapaNET;

public class ChapaRequest
{
    public double Amount { get; set; }
    public string Currency { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string TransactionReference { get; set; }
    public string? CallbackUrl { get; set; }

    public string? ReturnUrl
    {
        get
        {
            if (returnUrl is not null && (!returnUrl.StartsWith("https://") && !returnUrl.StartsWith("http://")))
            {
                returnUrl = "http://" + returnUrl;
            }
            return returnUrl;
        }
        set
        {
            returnUrl = value;
        }
    }

    string? returnUrl;

    public string? CustomTitle { get; set; }
    public string? CustomDescription { get; set; }
    public string? CustomLogo { get; set; }

    public ChapaRequest(double amount, string email
        , string first_name, string last_name
        , string tx_ref
        , RequestCustomization customization
        , string? callback_url = null
        , string? return_url = null
        , string currency = "ETB"
        )
    {
        Amount = amount;
        Currency = currency;
        Email = email;
        FirstName = first_name;
        LastName = last_name;
        TransactionReference = tx_ref;
        CallbackUrl = callback_url;
        ReturnUrl = return_url;
        CustomTitle = customization.Title;
        CustomDescription = customization.Description;
        CustomLogo = customization.Logo;
    }
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
public class RequestCustomization
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Logo { get; set; }
    public RequestCustomization(string? title = null, string? description = null, string? logo = null)
    {
        Title = title;
        Description = description;
        Logo = logo;
    }
    public static RequestCustomization Create(string? title = null, string? description = null, string? logo = null) => new(title, description, logo);
    public static RequestCustomization None = new();
}
