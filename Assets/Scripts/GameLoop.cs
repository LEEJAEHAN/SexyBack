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
        GameCameras gameSetting;

        // Use this for initialization
        void Awake()
        {
            viewLoader = new ViewLoader();
            viewLoader.RemoveTestObject();

            gameSetting = new GameCameras();

            tableLoader = Singleton<TableLoader>.getInstance();
            tableLoader.Init();

            gameInput = Singleton<GameInput>.getInstance();

            gameManager = Singleton<GameManager>.getInstance();

            gameManager.Init(new GameModeData("teststage", 100, 0));
        }
//        GameModeData args;
        void Start()
        {
            gameManager.Start();
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