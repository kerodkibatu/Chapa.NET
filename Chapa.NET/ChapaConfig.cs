
namespace ChapaNET;
public class ChapaConfig
{
    public const string AUTH_HEADER = "Authorization";
    public const string ACCEPTS_ENCODING_HEADER = "acceptEncodingHeader";

    public string API_SECRET { get; set; } = string.Empty;
    public const string BASE_PATH = "https://api.chapa.co/v1";

}