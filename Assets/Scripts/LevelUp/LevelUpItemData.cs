namespace SexyBackPlayScene
{
    public class LevelUpItemData
    {
        public string OwnerID; // == levelup item 의 id
        public string IconName;
        public string InfoName;

        public LevelUpItemData(string ownerID, string infoName, string iconName)
        {
            OwnerID = ownerID;
            InfoName = infoName;
            IconName = iconName;
        }
    }
}