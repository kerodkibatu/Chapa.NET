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

