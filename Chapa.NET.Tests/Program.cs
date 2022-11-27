//Import Chapa.NET
using ChapaNET;

//Initialize your Chapa Instance
Chapa chapa = new("CHASECK_TEST-Y8X7lsLcH4QY5yr2OM7kJ1bMe1qE7o9O");

//Get a unique transaction ID
var ID = Chapa.GetUniqueTransactionRef();

//Make a request
var request = new ChapaRequest(
    amount: 30.52
    , email: "abebebikila@gmail.com"
    , first_name: "Abebe"
    , last_name: "Bikila"
    , tx_ref: ID
    , return_url: "google.com");

//Process the request and get a response asynchronously
var Result = chapa.Request(request);


//Print out the checkout link
Console.WriteLine(Result.CheckoutUrl);

//Wait For 30sec
await Task.Delay(TimeSpan.FromSeconds(30));


//Verify Transaction
Console.WriteLine(chapa.Verify(ID));