namespace SexyBackPlayScene
{
    public interface IHasGridItem
    {
        void onSelect(string id);
        void onConfirm(string id);
        void onPause(string id);
        void Refresh();
    }
}

