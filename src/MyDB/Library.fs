namespace MyDB

open FSharp.Data.Dapper
open Microsoft.Data.Sqlite
open System.IO
open System

module Connection =
  let private curDir = Directory.GetCurrentDirectory() + "/"
  let private mkMemoryConnectionString datasource =
    sprintf "Data Source = %s; Mode = Memory; Cache = Shared;" datasource
  let private mkConnectionString (dataSource : string) =
    sprintf "Data Source = %s;" dataSource
  let private mkDedicatedConnectionString () =
    mkMemoryConnectionString (sprintf "DEDICATED -> %s" (Guid.NewGuid().ToString()))
  let mkMemoryShared =
    new SqliteConnection (mkDedicatedConnectionString())
  let mkSharedDBName name =
    new SqliteConnection (mkConnectionString (curDir + name))

  let mkSharedDBNameF name =
    Connection.SqliteConnection (mkSharedDBName name)
  let mkMemorySharedF con =
    Connection.SqliteConnection con

module Types =
    [<CLIMutable>]
    type Person =
        { Id         : int64
          Name       : string
          }

module Queries =
  let InitializeTable con =
    let querySingleOptionAsync:QuerySingleOptionAsyncBuilder<Types.Person> =
        (querySingleOptionAsync<Types.Person> con)
    querySingleOptionAsync {
      script """
create table Person(
      id integer primary key,
      name text not null
);
insert into Person(name) values("hoge");
insert into Person(name) values("fuga");
      """
    }

  module Person =
    let FindByName con name =
      let querySingleAsync:QuerySingleAsyncBuilder<Types.Person> =
        (querySingleAsync<Types.Person> con)
      querySingleAsync {
        script "select * from Person where Name = @Name limit 1"
        parameters (dict ["Name", box name])
      }

    let TryFindByName con name =
      let querySingleOptionAsync:QuerySingleOptionAsyncBuilder<Types.Person> =
        (querySingleOptionAsync<Types.Person> con)
      querySingleOptionAsync {
        script "select * from Person where Name = @Name limit 1"
        parameters (dict ["Name", box name])
      }

    let InsertPerson con name =
      let querySingleOptionAsync:QuerySingleOptionAsyncBuilder<Types.Person> =
        (querySingleOptionAsync<Types.Person> con)
      querySingleOptionAsync {
        script "insert into Person(name) values(@Name)"
        parameters (dict ["Name", box name])
      }
