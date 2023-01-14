module derpmap

open FSharp.Data.LiteralProviders
open FsHttp
open Model
open Thoth

let [<Literal>] Default = TextFile<"../default.json">.Text
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

let getDefaultDerpmap () : Derpmap =
    Default
    |> decodeDerpmap
    |> function
        | Some derpmap ->
            derpmap
        | None ->
            failwith "Failed to decode default derpmap."

let parseDerpmap (derpmap : Derpmap) : string =
    derpmap.Regions
    |> Map.values
    |> Seq.map (fun region ->
        region.Nodes
        |> List.map (fun node ->
            $"%s{node.HostName}, %s{node.IPv4}"
        )
    )
    |> Seq.concat
    |> Seq.fold (fun acc line -> acc + line + System.Environment.NewLine) System.String.Empty

let writeCsv (s : string) : unit =
    System.IO.File.WriteAllText(csv, s)

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
