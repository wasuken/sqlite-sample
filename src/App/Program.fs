// Learn more about F# at http://fsharp.org

open System
open System.IO
open MyDB
open MyDB.Connection

[<EntryPoint>]
let main argv =
  let curDir = Directory.GetCurrentDirectory() + "/"
  let con = mkSharedF
  let result = match (File.Exists(curDir + "prod.sqlite")) with
    | false -> Some (Queries.InitializeTable con |> Async.RunSynchronously)
    | true -> None
  let person = Queries.Person.TryFindByName con "hoge" |> Async.RunSynchronously
  printfn "%s" person.Value.Name
  0 // return an integer exit code
