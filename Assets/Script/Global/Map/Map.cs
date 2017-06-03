public class Map
{
    public int ClearCount;
    public int BestTime;
    public MapData baseData;
    public string ID { get { return baseData.ID; } }

    public Map(MapData data)
    {
        baseData = data;
        ClearCount = 0;
        BestTime = 0;
    }
}