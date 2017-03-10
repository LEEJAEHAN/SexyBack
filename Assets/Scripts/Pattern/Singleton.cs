using System;

namespace SexyBackPlayScene
{
    class Singleton<T> where T : class, new() // ISingleton, IDisposable, 
    {
        public static T Instance;
        public static T getInstance()
        {
            if (Instance == null)
                Instance = new T();
            return Instance;
        }

        public static void Clear()
        {
            Instance = null;
        }
        // Use this for initialization
        //        abstract protected void Init();

    }
}