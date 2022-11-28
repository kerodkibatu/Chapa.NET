# Chapa.NET
[![NuGet version (Krane)](https://img.shields.io/nuget/v/Chapa.NET.svg)](https://www.nuget.org/packages/Chapa.NET)
[![GitHub](https://img.shields.io/github/license/kerodkibatu/chapa.net)](https://www.gnu.org/licenses/gpl-3.0.txt)

# Chapa

Chapa is an Ethiopian Financial Service and Data Engineering Company. The inevitable increase in global trade which has been visibly troubled by inconvenient payment methods served as the strongest initiative behind the establishment of Chapa.

## Installation

```powershell
dotnet add package Chapa.NET
```

## Usage

Simple transaction creation using C#

```csharp
//Import Chapa.NET
using ChapaNET;

//Initialize your Chapa Instance
Chapa chapa = new("CHASECK_TEST-JY0ePBSclgj9KQJjjbb0vJD2ixpyI2KI");

//Get a unique transaction ID
var ID = Chapa.GetUniqueRef();


//Get Banks
Console.WriteLine("-----Fetching Banks------");

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
Console.WriteLine("Checkout Url:"+Result.Urls?.CheckoutUrl);


//Wait For 1min
Console.WriteLine("-----Waiting For 30sec------");
await Task.Delay(TimeSpan.FromSeconds(5));

//Verify Transaction - temporarly not working
Console.WriteLine("-----Verifying Transaction------");
Validity isValid = await chapa.VerifyAsync(ID);
Console.WriteLine("Validity: "+isValid);
```
## Mission
Providing Secure and Simple to use Chapa SDK for dotNET languages

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[GNU General Public License v3.0](https://www.gnu.org/licenses/gpl-3.0.txt)
