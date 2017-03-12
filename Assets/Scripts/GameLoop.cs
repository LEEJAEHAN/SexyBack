using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

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
            gameSetting = new GameCameras();
            tableLoader = Singleton<TableLoader>.getInstance();
            gameInput = Singleton<GameInput>.getInstance();
            gameManager = Singleton<GameManager>.getInstance();

            viewLoader.Init();
            tableLoader.Init();

            gameManager.Init();
        }
            //        GameModeData args;
        void Start()
        {
            if (PlayerPrefs.HasKey("InstanceData"))
            {
                gameManager.LoadInstance();
            }
            {
                gameManager.Start();
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
        public void userPause()
        {
            gameManager.Pause(true);
        }
        public void userUnPause()
        {
            gameManager.Pause(false);
        }
        public void EndGame() // 메뉴로 버튼을 눌렀을때,
        {
            // TODO : 게임매니저. 게임클리어에서 보상절차 작업해야함.
            gameManager.GameClear();                  // 보상절차과정
            gameManager.ClearInstance(); // 인스턴스세이브 데이터 지움.
            SceneManager.LoadScene("MenuScene");
        }
        private void OnDestroy()
        {
            //tableLoader.Dispose();            // 테이블 로더는 남겨둔다.
            //Singleton<TableLoader>.Clear();
            sexybacklog.Console("플레이씬 디스트로이");
            gameInput.Dispose();
            gameManager.Dispose();
            Singleton<GameInput>.Clear();
            Singleton<GameManager>.Clear();
        }
        public void Quit()
        {
            Application.Quit();
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