module database_functions

open BrightstarDB.Client
open System.Text

let testing =
    let dbClient = BrightstarService.GetClient("Type=embedded;storesdirectory=/home/paulm/Documents/Data")
    let storeName = "bs_kernai"
    dbClient.CreateStore(storeName)
    let transactionData = new UpdateTransactionData()
    let uri = "<http://www.parinherm.com/schemas/product/name>"
    let addTriples = new StringBuilder();
    addTriples.AppendLine( sprintf "%s/project/name \"BrightstarDB\" ." uri) |> ignore
    addTriples.AppendLine("<http://www.brightstardb.com/schemas/product/category> <http://www.brightstardb.com/categories/nosql> .") |> ignore
    addTriples.AppendLine("<http://www.brightstardb.com/schemas/product/category> <http://www.brightstardb.com/categories/.net> .") |> ignore
    addTriples.AppendLine("<http://www.brightstardb.com/schemas/product/category> <http://www.brightstardb.com/categories/rdf> .") |> ignore

    let transactionData = new UpdateTransactionData ()
    transactionData.InsertData <- addTriples.ToString()
    let jobinfo = dbClient.ExecuteTransaction(storeName, transactionData)

    printfn("opening database here") |> ignore