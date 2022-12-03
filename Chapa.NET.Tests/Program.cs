//Import Chapa.NET
using ChapaNET;

//Initialize your Chapa Instance
string APIKEY = "YOUR API KEY";
Chapa chapa = new(APIKEY);

//Get a unique transaction ID
var ID = Chapa.GetUniqueRef();


//Get Banks
Console.WriteLine("-----Fetching Banks------");
await chapa.GetBanksAsync();
var banks = await chapa.GetBanksAsync();
Console.WriteLine(string.Join("\n------",banks.AsEnumerable()));

//Make a request

Console.WriteLine("-----Making A Request------");
var Request = new ChapaRequest(
    amount: 8000,
    email: "kibatuwsenbet101@gmail.com",
    firstName: "Kibatu",
    lastName: "W/Senbet",
    tx_ref: ID
    );


//Process the request and get a response asynchronously
var Result = await chapa.RequestAsync(Request);



//Print out the checkout link
Console.WriteLine("Checkout Url:"+Result.CheckoutUrl);



//Give Time To Complete Transaction
int timeToWait = 60;
Console.WriteLine("-----Waiting For Completion------");
var rowInit = Console.CursorTop;
for (int i = timeToWait - 1; i >= 0; i--)
{
    Console.WriteLine($"{i}s till validation");
    Console.CursorTop = rowInit-1;
    await Task.Delay(1000);
}


//Verify Transaction - temporarly not working
Console.WriteLine("-----Verifying Transaction------");
Validity isValid = await chapa.VerifyAsync(ID);
Console.WriteLine("Validity: "+isValid);