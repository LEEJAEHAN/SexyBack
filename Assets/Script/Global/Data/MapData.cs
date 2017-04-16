
internal class MapData
{
    public string ID;
    public string Name;
    public int Level;
    public int LimitTime;
    public int LimitLevel;
    public int LimitResearch;
    public int MaxFloor;

    public MapData(string stageid, int maplevel, string name, int maxfloor, int limitTime, int limitLevel, int limitResearch)
    {
        ID = stageid;
        this.Name = name;
        Level = maplevel;
        LimitTime = limitTime;
        MaxFloor = maxfloor;
        LimitLevel = limitLevel;
        LimitResearch = limitResearch;
    }
}