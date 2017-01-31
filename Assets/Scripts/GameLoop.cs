using UnityEngine;
using System.Collections;
using System;

namespace SexyBackPlayScene
{

    public class GameLoop : MonoBehaviour
    {
        ViewLoader viewLoader;
        TableLoader tableLoader;
        GameManager gameManager;
        GameInput gameInput;
        GameSetting gameSetting;

        // Use this for initialization
        void Awake()
        {
            viewLoader = new ViewLoader();

            gameSetting = new GameSetting();
            gameSetting.Init();

            tableLoader = Singleton<TableLoader>.getInstance();
            tableLoader.LoadAll();

            gameInput = Singleton<GameInput>.getInstance();

            gameManager = Singleton<GameManager>.getInstance();
            gameManager.Init();
        }
        object args;
        void Start()
        {
            gameManager.Start(args);
        }
        // Update is called once per frame
        void Update()
        {
            gameManager.Update();
            gameInput.CheckInput();
        }

        private void FixedUpdate()
        {
            gameManager.FixedUpdate();
        }

    }

}