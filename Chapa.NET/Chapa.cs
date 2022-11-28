using Flurl;
using Flurl.Http;

namespace ChapaNET;
public enum Validity { Valid, Invalid }
public class Chapa
{
    public static string GetUniqueTransactionRef() => Guid.NewGuid().ToString();
    ChapaConfig Config { get; set; }
    public static string GetUniqueRef() => Guid.NewGuid().ToString();
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

        var response = await "https://api.chapa.co/v1"
            .WithHeader(ChapaConfig.AUTH_HEADER, $"Bearer {Config.API_SECRET}")
            .AppendPathSegments("transaction", "initialize")
            .PostJsonAsync(
                reqDict
            )
            .ReceiveJson<ChapaResponse>();
        return response;
    }
    public async Task<Validity> VerifyAsync(string txRef)
    {
        return
            (await "https://api.chapa.co/v1"
            .WithHeader(ChapaConfig.AUTH_HEADER, $"Bearer {Config.API_SECRET}")
            .AppendPathSegments("transaction", "verify", txRef)
            .GetAsync())
            .StatusCode == (int)System.Net.HttpStatusCode.OK ? Validity.Valid : Validity.Invalid;
    }
    public async Task<IEnumerable<Bank>> GetBanksAsync()
    {
        var response = 
            await "https://api.chapa.co/v1"
                .WithHeader(ChapaConfig.AUTH_HEADER, $"Bearer {Config.API_SECRET}")
                .AppendPathSegment("banks")
                .GetAsync();

        return JsonConvert.DeserializeObject<BankResponse>(await response.GetStringAsync())!.data.DistinctBy(x => x.Name);
    }
    class BankResponse
    {
        public string? message { get; set; }

        public IEnumerable<Bank> data { get; set; }
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
        return $"{Name}";
    }
    public async Task<Validity> VerifyAsync(string TransactionReference)
    {
        return await Task.FromResult(Verify(TransactionReference));
    }
    public static RestRequest MakeRestRequest(ChapaRequest chapaReq)
    {
        var request = new RestRequest("/transaction/initialize", Method.Post)
                .AddParameter("email", chapaReq.Email)
                .AddParameter("amount", chapaReq.Amount)
                .AddParameter("first_name", chapaReq.FirstName)
                .AddParameter("last_name", chapaReq.LastName)
                .AddParameter("tx_ref", chapaReq.TransactionReference)
                .AddParameter("currency", chapaReq.Currency);
        if (chapaReq.CustomTitle is not null)
            request.AddParameter($"customization[title]",chapaReq.CustomTitle);
        if (chapaReq.CustomDescription is not null)
            request.AddParameter($"customization[description]",chapaReq.CustomDescription);
        if (chapaReq.CustomLogo is not null)
            request.AddParameter($"customization[logo]",chapaReq.CustomLogo);
        if (chapaReq.CallbackUrl is not null)
            request.AddParameter("callback_url", chapaReq.CallbackUrl);
        if (chapaReq.ReturnUrl is not null)
            request.AddParameter("return_url", chapaReq.ReturnUrl);
        return request;
    }
}

