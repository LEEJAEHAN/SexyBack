namespace SexyBackPlayScene
{
    internal interface IStateOwner
    {
        string GetID { get; }
        string CurrentState { get; }
    }
}