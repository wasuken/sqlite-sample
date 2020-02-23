module MyDBTest

open NUnit.Framework
open MyDB
open System.IO

[<SetUp>]
let Setup () =
    ()

[<Test>]
let Test1 () =
  let curDir = Directory.GetCurrentDirectory() + "/"
  match (File.Exists(curDir + "test.sqlite")) with
    | false -> Some (File.Delete(curDir + "test.sqlite"))
    | true -> None
  let con = (fun unit -> Connection.mkSharedDBNameF "test.sqlite")
  Queries.InitializeTable con |> Async.RunSynchronously |> ignore
  let person = Queries.Person.TryFindByName  con "hoge" |> Async.RunSynchronously
  Assert.True(person.Value.Name.Equals("hoge"))
  File.Delete(curDir + "test.sqlite")
