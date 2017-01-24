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
        GameSetting gameSetting;

        // Use this for initialization
        void Awake()
        {
            viewLoader = new ViewLoader();
            gameSetting = new GameSetting();
            gameSetting.Init();
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

        private void FixedUpdate()
        {
            gameManager.FixedUpdate();
        }

    }

}