//Import Chapa.NET
using ChapaNET;

//Initialize your Chapa Instance
Chapa chapa = new("CHASECK_TEST-JY0ePBSclgj9KQJjjbb0vJD2ixpyI2KI");

//Get a unique transaction ID
var ID = Chapa.GetUniqueRef();


//Get Banks
Console.WriteLine("-----Fetching Banks------");
await chapa.GetBanksAsync();
var banks = await chapa.GetBanksAsync();
Console.WriteLine(string.Join(',',banks.AsEnumerable()));

//Make a request

Console.WriteLine("-----Making A Request------");
var Request = new ChapaRequest(
    amount: 3500,
    email: "abebebikila@gmail.com",
    firstName: "Abebe",
    lastName: "Bikila",
    tx_ref: ID,
    callback_url: "https://google.com"
    );


//Process the request and get a response asynchronously
var Result = await chapa.RequestAsync(Request);

//Print out the checkout link
Console.WriteLine("Checkout Url:"+Result.CheckoutUrl);

//Give Time To Complete Transaction
Console.WriteLine("-----Waiting For Completion------");
await Task.Delay(TimeSpan.FromMinutes(1));


//Verify Transaction - temporarly not working
Console.WriteLine("-----Verifying Transaction------");
Validity isValid = await chapa.VerifyAsync(ID);
Console.WriteLine("Validity: "+isValid);