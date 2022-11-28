using Flurl;
using Flurl.Http;
using Newtonsoft.Json;

namespace ChapaNET;
public enum Validity { Valid, Invalid }
public class Chapa
{
    ChapaConfig Config { get; set; }
    public static string GetUniqueRef() => "tx" + DateTime.Now.ToBinary();
    public Chapa(string SECRET_KEY)
    {
        Config = new() { API_SECRET = SECRET_KEY };

        //Client = new RestClient(ChapaConfig.BASE_PATH)
        //    .AddDefaultHeader(ChapaConfig.AUTH_HEADER, "Bearer " + Config.API_SECRET)
        //    .UseNewtonsoftJson();
    }
    public Chapa(ChapaConfig config) : this(config.API_SECRET) { }
    public async Task<ChapaResponse> RequestAsync(ChapaRequest request)
    {
        var reqDict = new Dictionary<string, string>()
        {
            {"email",request.Email},
            {"amount",request.Amount.ToString()},
            {"first_name",request.FirstName},
            {"last_name", request.LastName},
            {"tx_ref",request.TransactionReference},
            {"currency",request.Currency},
        };

        if (request.CallbackUrl != null)
            reqDict.Add("callback_url", request.CallbackUrl);
        if (request.ReturnUrl != null)
            reqDict.Add("return_url", request.ReturnUrl);
        if (request.CustomTitle != null)
            reqDict.Add("customization[title]", request.CustomTitle);
        if (request.CustomDescription != null)
            reqDict.Add("customization[description]", request.CustomDescription);
        if (request.CustomLogo != null)
            reqDict.Add("customization[logo]", request.CustomLogo);

        var response = await "https://api.chapa.co/v1/transaction/initialize"
            .WithHeader(ChapaConfig.AUTH_HEADER, $"Bearer {Config.API_SECRET}")
            .PostJsonAsync(reqDict)
            .ReceiveJson();
        
        return new() 
        {
            Status = response.status,
            Message = response.message,
            CheckoutUrl = response.data.checkout_url
        };
    }
    public async Task<Validity> VerifyAsync(string txRef)
    {
        Validity validity = Validity.Valid;
        try
        {
            await $"https://api.chapa.co/v1/transaction/verify/{txRef}"
                        .WithHeader(ChapaConfig.AUTH_HEADER, $"Bearer {Config.API_SECRET}")
                        .GetAsync();
        }
        catch (Exception)
        {
            validity = Validity.Invalid;
        }
        return validity;
    }
    public async Task<IEnumerable<Bank>> GetBanksAsync()
    {
        var response = 
            await "https://api.chapa.co/v1"
                .WithHeader(ChapaConfig.AUTH_HEADER, $"Bearer {Config.API_SECRET}")
                .AppendPathSegment("banks")
                .GetJsonAsync<BankResponse>();

        return response.data!;
    }
    class BankResponse
    {
        public string? message { get; set; }

        public IEnumerable<Bank>? data { get; set; }
    }
}
public class Bank
{
    public string ID;
    public string SwiftCode;
    public string Name;
    public int AccLen;
    public int CountryID;
    public Bank(string id,string swift, string name,int accLen,int country_id)
    {
        ID = id;
        SwiftCode = swift;
        Name = name;
        AccLen = accLen;
        CountryID = country_id;
    }
    public override string ToString()
    {
        return 
        $@"ID: {ID}
Name: {Name}
Swift Code: {SwiftCode}
AcctLen: {AccLen}
Country ID: {CountryID}";
    }
}

