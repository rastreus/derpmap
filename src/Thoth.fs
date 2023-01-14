module Thoth

open Model
open Thoth.Json.Net

let nodeDecoder : Decoder<Node> =
    Decode.object (fun get ->
        { Name = get.Required.Field "Name" Decode.string
          RegionID = get.Required.Field "RegionID" Decode.int
          HostName = get.Required.Field "HostName" Decode.string
          IPv4 = get.Required.Field "IPv4" Decode.string
          IPv6 = get.Required.Field "IPv6" Decode.string }
    )

let regionDecoder : Decoder<Region> =
    Decode.object (fun get ->
        { RegionID = get.Required.Field "RegionID" Decode.int
          RegionCode = get.Required.Field "RegionCode" Decode.string
          RegionName = get.Required.Field "RegionName" Decode.string
          Nodes = get.Required.Field "Nodes" (Decode.list nodeDecoder) }
    )

let derpmapDecoder : Decoder<Derpmap> =
    Decode.object (fun get ->
        { Regions = get.Required.Field "Regions" (Decode.dict regionDecoder) }
    )

let decodeDerpmap (json : string) : Derpmap option =
    try
        let decodeResult : Result<Derpmap, string> =
            Decode.fromString derpmapDecoder json
        match decodeResult with
        | Ok derpmap -> Some derpmap
        | Error error ->
            printfn $"[decodeResult] error: %s{error}"
            None
    with
        | exn ->
            printfn $"[decodeDerpmap] exn.Message: %s{exn.Message}"
            None
