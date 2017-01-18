using UnityEngine;
using System.Collections;
using System;

namespace SexyBackPlayScene
{

    public class GameLoop : MonoBehaviour
    {
        ViewLoader viewLoader;
        GameManager gameManager;
        GameInput gameInput;
        GameSetting gameSetting = new GameSetting();

        // Use this for initialization
        void Awake()
        {
            ViewLoader viewLoader = new ViewLoader();
            gameSetting.Init();
            gameSetting.RemoveTestObject();
        }

        void Start()
        {
            gameManager = Singleton<GameManager>.getInstance();
            gameManager.Init();
            gameInput = Singleton<GameInput>.getInstance();

        }
        // Update is called once per frame
        void Update()
        {
            gameManager.Update();
            gameInput.CheckInput();
        }

    }

}