using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace ChapaNET;
public enum Validity { Valid, Invalid }
public class Chapa
{
    public static string GetUniqueTransactionRef() => Guid.NewGuid().ToString();
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

    public ChapaResponse Request(ChapaRequest chapaRequest)
    {
        var response = Client.Post<ChapaResponse>(MakeRestRequest(chapaRequest));
        return response!;
    }
    public async Task<ChapaResponse> RequestAsync(ChapaRequest chapaReq)
    {
        return await Task.FromResult(Request(chapaReq));
    }
    public Validity Verify(string TransactionReference)
    {
        return Client.Get(new($"/transaction/verify/{TransactionReference}")).StatusCode == System.Net.HttpStatusCode.OK ? Validity.Valid : Validity.Invalid;
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
            request.AddParameter("customization[title]", chapaReq.CustomTitle);
        if (chapaReq.CustomDescription is not null)
            request.AddParameter("customization[description]", chapaReq.CustomDescription);
        if (chapaReq.CustomLogo is not null)
            request.AddParameter("customization[logo]", chapaReq.CustomLogo);
        if (chapaReq.CallbackUrl is not null)
            request.AddParameter("callback_url", chapaReq.CallbackUrl);
        if (chapaReq.ReturnUrl is not null)
            request.AddParameter("return_url", chapaReq.ReturnUrl);
        return request;
    }
    /// <param name="chapaRequest"></param>
    /// <summary>
    /// Makes a request to the chapa api using information from <paramref name="chapaRequest"/>
    /// <code>
    /// Example Usage:
    /// var chapa = new <see cref="Chapa"/>("*Your-API-Key*");
    /// string ID = <see cref="GetUniqueTransactionRef()"/>;
    /// 
    /// var request = new <see cref="ChapaRequest"/>(
    ///     amount: 100
    ///     , email: "abebebikila@gmail.com"
    ///     , first_name: "Abebe"
    ///     , last_name: "Bikila"
    ///     , tx_ref: ID);
    /// 
    /// var Result = chapa.Request(request);
    /// </code>
    /// </summary>
    /// <returns>The response in the form of a <see cref="ChapaResponse"/> object</returns>
}

