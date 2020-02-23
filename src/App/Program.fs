// Learn more about F# at http://fsharp.org

open System
open System.IO
open MyDB
open MyDB.Connection

[<EntryPoint>]
let main argv =
  let curDir = Directory.GetCurrentDirectory() + "/"
  let con = (fun unit -> mkSharedDBNameF "prod.sqlite")
  let result = match (File.Exists(curDir + "prod.sqlite")) with
    | false -> Some (Queries.InitializeTable con |> Async.RunSynchronously) |> ignore
    | true -> None
  let person = Queries.Person.TryFindByName con "hoge" |> Async.RunSynchronously
  printfn "%s" person.Value.Name
  0 // return an integer exit code
