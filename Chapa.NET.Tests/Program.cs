//Import Chapa.NET
using ChapaNET;

//Initialize your Chapa Instance
Chapa chapa = new("CHASECK_TEST-JY0ePBSclgj9KQJjjbb0vJD2ixpyI2KI");

//Get a unique transaction ID
var ID = Chapa.GetUniqueTransactionRef();

//Make a request
var request = new ChapaRequest(
      amount: 30.52
    , email: "abebebikila@gmail.com"
    , first_name: "Abebe"
    , last_name: "Bikila"
    , tx_ref: ID
    , customization: RequestCustomization.None
    , return_url: "https://www.google.com");

Console.WriteLine(request);

//Process the request and get a response asynchronously
var result = chapa.Request(request);

Console.WriteLine("\n-----Response-----");

//Print out the checkout link
Console.WriteLine(result);

//Wait For 30sec
await Task.Delay(TimeSpan.FromSeconds(30));

//Verify Transaction
Console.WriteLine(chapa.Verify(ID));