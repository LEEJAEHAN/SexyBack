namespace SexyBackPlayScene
{
    class Singleton<T> where T : class, new()
    {
        public static T Instance;
        public static T getInstance()
        {
            if (Instance == null)
                Instance = new T();
            return Instance;
        }

        public virtual void Clear()
        {
            Instance = null;
            Instance = new T();
        }
        // Use this for initialization
//        abstract protected void Init();

    }
}