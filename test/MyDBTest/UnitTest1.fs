module MyDBTest

open NUnit.Framework
open MyDB

[<SetUp>]
let Setup () =
    ()

[<Test>]
let Test1 () =
    let con = Connection.mkMemoryShared
    con.Open()
    let con2 = (fun unit -> Connection.mkMemorySharedF(con))
    Queries.InitializeTable con2 |> Async.RunSynchronously |> ignore
    let person = Queries.Person.TryFindByName  con2 "hoge" |> Async.RunSynchronously
    Assert.True(person.Value.Name.Equals("hoge"))
    con.Close()
