namespace PlcTryCommon;

public class Variable
{
    public Variable(string name, string type, string mapId)
    {
        Name = name;
        Type = type;
        MapId = mapId;
    }

    public string Name { get; set; }
    public string Type { get; set; }
    public string MapId { get; set; }
}