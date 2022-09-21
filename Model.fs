module Model

type Node =
    {
        Name : string
        RegionID : int
        HostName : string
        IPv4 : string
        IPv6 : string
    }

type Region =
    {
        RegionID : int
        RegionCode : string
        RegionName : string
        Nodes : Node list
    }

type Derpmap =
    {
        Regions : Map<string, Region>
    }
