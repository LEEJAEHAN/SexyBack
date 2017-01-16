namespace SexyBackPlayScene
{
    public class LevelUpItemData
    {
        public string OwnerID;
        public int PurchasedCount; // 조정될 레벨
        public string IconName;
        public string InfoName;

        public LevelUpItemData(string ownerID, string infoName, string iconName, int purchasedCount)
        {
            OwnerID = ownerID;
            InfoName = infoName;
            IconName = iconName;
            PurchasedCount = purchasedCount;
        }
    }
}