namespace SexyBackPlayScene
{
    public class LevelUpData
    {
        public string Order;
        public string OwnerID; // == levelup item 의 id
        public string IconName;
        public string OwnerName;

        public LevelUpData(string ownerID, string infoName, string iconName, string order)
        {
            OwnerID = ownerID;
            OwnerName = infoName;
            IconName = iconName;
            Order = order;
        }
    }
}
