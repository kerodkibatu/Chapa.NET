//Import Chapa.NET
using ChapaNET;

//Initialize your Chapa Instance
Chapa chapa = new Chapa("CHASECK-*YOUR-API-KEY*");

//Get a unique transaction ID
var ID = Guid.NewGuid().ToString();

//Make a request
var Request = new ChapaRequest(amount: 30.52, email: "abebebikila@gmail.com", first_name: "Abebe", last_name: "Bikila", tx_ref: ID);

//Process the request and get a response asynchronously
var Result = await chapa.Request(Request);

//Print out the checkout link
Console.WriteLine(Result.Urls?.CheckoutUrl);

//Wait For 1min
Thread.Sleep(TimeSpan.FromMinutes(1));

//Verify Transaction
Console.WriteLine(chapa.Verify(ID));