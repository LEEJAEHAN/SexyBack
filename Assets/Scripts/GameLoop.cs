using UnityEngine;
using System.Collections;

namespace SexyBackPlayScene
{
    public class GameLoop : MonoBehaviour
    {
        GameManager gameManager;



        // Use this for initialization
        void Start()
        {
            gameManager = Singleton<GameManager>.getInstance();
            gameManager.Init();

            ViewLoader.Item_Enable.transform.DestroyChildren();
        }

        // Update is called once per frame
        void Update()
        {
            gameManager.Update();
        }
}

}