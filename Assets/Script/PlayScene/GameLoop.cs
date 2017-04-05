using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

namespace SexyBackPlayScene
{
    public class GameLoop : MonoBehaviour
    {
        ViewLoader viewLoader;
        GameManager gameManager;
        GameInput gameInput;
        GameCameras cameraSetting;

        // Use this for initialization
        void Awake()
        {
            viewLoader = new ViewLoader();
            cameraSetting = new GameCameras();
            Singleton<TableLoader>.getInstance().Init();
            gameInput = Singleton<GameInput>.getInstance();
            gameManager = Singleton<GameManager>.getInstance();

            gameManager.Init();
        }
            //        GameModeData args;
        void Start()
        {
            if (SaveSystem.CanLoad())
            {
                //gameManager.NewInstance();
                gameManager.LoadInstance();
            }
            else // 메뉴로부터실행됬을때
            {
                gameManager.NewInstance();
            }
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
        public void Option()
        {
            gameManager.Pause(true);
            ViewLoader.PopUpPanel.SetActive(true);
            ViewLoader.OptionPanel.SetActive(true);
        }
        public void ExitOption()
        {
            gameManager.Pause(false);
            ViewLoader.OptionPanel.SetActive(false);
            ViewLoader.PopUpPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            sexybacklog.Console("플레이씬 디스트로이");
            gameInput.Dispose();
            gameManager.Dispose();
            Singleton<GameInput>.Clear();
            Singleton<GameManager>.Clear();
        }
        private void OnApplicationQuit()
        {
            sexybacklog.Console("어플강제종료");
            gameManager.SaveInstance(); // 인스턴스세이브
        }
        private void OnApplicationPause(bool pause)
        {
            // 별다른 처리안해도 되는듯?
        }

    }

}