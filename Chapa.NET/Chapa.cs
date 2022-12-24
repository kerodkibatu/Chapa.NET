using Flurl;
using Flurl.Http;

namespace ChapaNET;
public enum TransactionStatus
{
    Completed, Incomplete, Error
}
public class Chapa
{
    ChapaConfig Config { get; set; }
    public static string GetUniqueRef() => "tx" + DateTime.Now.Ticks;
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
        var reqDict = new Dictionary<string, string?>()
        {
            {"email",request.Email},
            {"amount",request.Amount.ToString()},
            {"first_name",request.FirstName},
            {"last_name", request.LastName},
            {"tx_ref",request.TransactionReference},
            {"currency",request.Currency},
        };
        if (request.PhoneNo != null)
            reqDict.Add("phone_number", request.PhoneNo);
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
    public class ValidityReport
    {
        public bool IsSuccess => status == "success";
        public string message { get; set; }
        public string status { get; set; }
        public Data data { get; set; }
        public class Data
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string currency { get; set; }
            public int amount { get; set; }
            public int charge { get; set; }
            public string mode { get; set; }
            public string method { get; set; }
            public string type { get; set; }
            public string status { get; set; }
            public string reference { get; set; }
            public string tx_ref { get; set; }
            public Customization customization { get; set; }
            public object meta { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
        }

        public class Customization
        {
            public string title { get; set; }
            public string description { get; set; }
            public string logo { get; set; }
        }
    }
    public async Task<ValidityReport?> VerifyAsync(string txRef)
    {
        try
        {
            var validityResponse = await $"https://api.chapa.co/v1/transaction/verify/{txRef}"
                        .WithHeader(ChapaConfig.AUTH_HEADER, $"Bearer {Config.API_SECRET}")
                        .GetJsonAsync<ValidityReport>();
            return validityResponse;
        }
        catch (Exception)
        {
            return null;
        }
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
    public Bank(string id, string swift, string name, int accLen, int country_id)
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

