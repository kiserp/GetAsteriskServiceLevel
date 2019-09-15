// Learn more about F# at http://fsharp.org

open System
// open FSharp.Configuration
open AsterNET.Manager
open AsterNET.Manager.Action
open AsterNET.Manager.Response
open AsterNET.Manager.Event
open GetAsteriskServiceLevel.QueueData

let [<Literal>] configFile = "./app.config"
// type Settings = Appsettings<configFile>


[<EntryPoint>]
let main argv =
    let managerConnection = 
        ManagerConnection(
            "mypbx",
            5038,
            "statuser",
            "EinKennwort")
        //  ManagerConnection(Settings.Host,
        //     Settings.Port,
        //     Settings.Loging,
        //     Settings.Secrete)

    managerConnection.Login()

    let command = new CommandAction()
    command.Command <- "queue show"
    let managerResponse = managerConnection.SendAction(command) 
 
    let response = managerResponse :?> CommandResponse
    
   // for msg in response.Result do 
   //     printfn "%s" msg
    
    
    let parsed =
        response.Result
        |> Seq.toList
        |> ParseAsteriskOutputForQueueData 
    
    for item in parsed do
        printfn "%A" item
         
    printfn "Hello World from F#!"
    0 // return an integer exit code
