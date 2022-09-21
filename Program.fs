module derpmap

open FSharp.Data.LiteralProviders
open FsHttp
open Model
open Thoth

let [<Literal>] Default = TextFile<"derpmap.json">.Text
let url = """https://login.tailscale.com/derpmap/default"""
let csv = """derpmap.csv"""

let tryGetDerpmap () : Derpmap option =
    try
        http {
            GET url
            AcceptLanguage "en-US"
            config_timeoutInSeconds 10.0
        }
        |> Request.send
        |> Response.toText
        |> decodeDerpmap
        |> function
            | Some derpmap ->
                Some derpmap
            | None ->
                Default
                |> decodeDerpmap
    with
        | exn ->
            printfn $"[tryGetDerpmap] exn: %s{exn.Message}"
            Default
            |> decodeDerpmap

let parseDerpmap (derpmap : Derpmap) : string list =
    derpmap.Regions
    |> Map.values
    |> Seq.map (fun region ->
        region.Nodes
        |> List.map (fun node ->
            $"%s{node.HostName}, %s{node.IPv4}"
        )
    )
    |> Seq.concat
    |> Seq.toList

let writeCsv (lines : string list) : unit =
    System.IO.File.WriteAllLines(csv, lines)

[<EntryPoint>]
let main _ =
    tryGetDerpmap()
    |> function
        | Some derpmap ->
            derpmap
            |> parseDerpmap
            |> writeCsv
            printfn $"[derpmap] Success! %s{csv} created."
            0
        | None ->
            printfn "[derpmap] Failure .-."
            1
