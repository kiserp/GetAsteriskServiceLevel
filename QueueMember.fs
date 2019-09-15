module GetAsteriskServiceLevel.Queuedata

type QueueMembership = Dynamic | Static | RealTime | Other

let ParseMembership =
    function
    | "dynamic" -> Dynamic
    | "static"  -> Static
    | "realtime"-> RealTime
    | _         -> Other
    
type QueueMember = {
    InterfaceProtocol: string;
    InterfaceNumber: uint32;
    Penalty: uint32;
    Membership: QueueMembership;
    Paused: bool;
    
}