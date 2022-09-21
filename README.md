# derpmap

A simple exercise in F# to get json, decode it and then create a CSV file.

* [F#](https://fsharp.org)
* [Paket](https://fsprojects.github.io/Paket/index.html)
* [FSharp.Data.LiteralProviders](https://github.com/tarmil/FSharp.Data.LiteralProviders)
* [Thoth.Json.Net](https://thoth-org.github.io/Thoth.Json/#.Net-and-NetCore-support)
* [FsHttp](https://fsprojects.github.io/FsHttp/)

## Install pre-requisites

* [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

## Build and run

1. `dotnet tool restore`

2. `dotnet paket install`

3. `dotnet build derpmap.fsproj`

4. `dotnet run --project derpmap.fsproj`
