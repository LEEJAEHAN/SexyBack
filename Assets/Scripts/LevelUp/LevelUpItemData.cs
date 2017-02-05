namespace SexyBackPlayScene
{
    public class LevelUpItemData
    {
        public string ID;
        public string OwnerID; // == levelup item 의 id
        public string IconName;
        public string InfoName;

        public LevelUpItemData(string id, string ownerID, string infoName, string iconName)
        {
            ID = id;
            OwnerID = ownerID;
            InfoName = infoName;
            IconName = iconName;
        }
    }
}