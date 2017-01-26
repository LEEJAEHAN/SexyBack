namespace SexyBackPlayScene
{
    public interface StateOwner
    {
        string ID { get; }
        string CurrentState { get; }
    }
}