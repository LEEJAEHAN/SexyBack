namespace SexyBackPlayScene
{
    public class LevelUpData
    {
        public string ID;
        public string OwnerID; // == levelup item 의 id
        public string IconName;
        public string OwnerName;

        public LevelUpData(string id, string ownerID, string infoName, string iconName)
        {
            ID = id;
            OwnerID = ownerID;
            OwnerName = infoName;
            IconName = iconName;
        }
    }
}