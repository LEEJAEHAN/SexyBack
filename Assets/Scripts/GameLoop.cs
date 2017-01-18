using UnityEngine;
using System.Collections;
using System;

namespace SexyBackPlayScene
{

    public class GameLoop : MonoBehaviour
    {
        GameManager gameManager;
        GameInput gameInput;
        GameSetting gameSetting = new GameSetting();

        // Use this for initialization
        void Start()
        {
            RemoveTestObject();

            gameSetting.Init();

            gameManager = Singleton<GameManager>.getInstance();
            gameManager.Init();

            gameInput = Singleton<GameInput>.getInstance();
        }

        private void RemoveTestObject()
        {
            // remove useless objects; like prefab preview
            ViewLoader.Item_Enable.transform.DestroyChildren();
            ViewLoader.shooter.transform.DestroyChildren();

            GameObject.Destroy(GameObject.Find("monster"));
        }

        // Update is called once per frame
        void Update()
        {
            gameManager.Update();
            gameInput.CheckInput();
        }

    }

}