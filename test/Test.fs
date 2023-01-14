module derpmap.Test

open Expecto
open FSharp.Data.LiteralProviders
open derpmap

let [<Literal>] Csv = TextFile<"../default.csv">.Text

[<Tests>]
let test =
    testCase "parseDerpmap works correctly" <| fun _ ->
        Expect.equal (getDefaultDerpmap() |> parseDerpmap) Csv ""

[<EntryPoint>]
let main argv =
    runTestsInAssembly defaultConfig argv
