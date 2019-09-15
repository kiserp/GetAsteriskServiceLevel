module GetAsteriskServiceLevel.QueueData
open System

type CallStrategy =
    RrMemory
  | FewestCals
  | RingAll
  | LeastRecent
  | Random
  | Linear
  | Wrandom
  | Other
  
let ParseCallStrategy = function
    | "rrmemory" -> RrMemory 
    | "fewestcalls" -> FewestCals
    | "ringall" -> RingAll
    | "leastrecent" -> LeastRecent
    | "random" -> Random
    | "linear" -> Linear
    | "wrandom" -> Wrandom
    | _ -> Other
  
type Queue = {Name: String;
              CallsCount: uint32;
              Strategy: CallStrategy;
              HoldTime: uint32;
              TalkTime: uint32;
              Weight: uint32;
              CallsAswered: uint32;
              CallsLost: uint32;
              ServiceLevel: float32}


let ParseQueue queueData =
    match queueData with
    | [|name; "has"; nowCalling; "calls";
        _; _; "in"; strategy; "strategy";
        holdtime; "holdtime,"; talktime;
        "talktime),"; weight;answered;lost;
        sl;"within"; _|]
            when queueData.Length = 19 ->
        let _nowCalling = nowCalling |> uint32
        let _ht = (holdtime.[1..(holdtime.Length-2)]) |> uint32
        let _tt = (talktime.[0..(talktime.Length-2)]) |> uint32
        let _wt = (weight.[2..(weight.Length-2)]) |> uint32
        let _ca = (answered.[2..(answered.Length-2)]) |> uint32
        let _cl = (lost.[2..(lost.Length-2)]) |> uint32
        Some({Name = name; CallsCount = _nowCalling;
         Strategy = (ParseCallStrategy (strategy.Trim('\'')));
         HoldTime = _ht;
         TalkTime = _tt;
         Weight = _wt;
         CallsAswered = _ca;
         CallsLost = _cl;
         ServiceLevel = ((sl.Split(":") |> Array.last).Trim('%') |> float32)})
    | _ -> None
        
let rec ParseAsteriskOutputForQueueData (queueShowOutput: string list)  =
    match queueShowOutput with
    | [] -> []
    | _ -> (ParseQueue (queueShowOutput.Head.Split())) :: ParseAsteriskOutputForQueueData queueShowOutput.Tail
    
        
        
        
     