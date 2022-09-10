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
```
## Mission
Providing Secure and Simple to use Chapa SDK for dotNET languages

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[GNU General Public License v3.0](https://www.gnu.org/licenses/gpl-3.0.txt)
