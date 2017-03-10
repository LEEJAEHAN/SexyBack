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
            gameManager.GameClear();
            PlayerPrefs.DeleteKey("게임중저장데이터");
            SceneManager.LoadScene("MenuScene");
        }

        private void OnDestroy()
        {
            //TODO : 3개 Destory() 체크해야함.
            //tableLoader.Dispose();            // 테이블 로더는 남겨둔다.
            //Singleton<TableLoader>.Clear();

            gameInput.Dispose();
            gameManager.Dispose();

            Singleton<GameInput>.Clear();
            Singleton<GameManager>.Clear();

            sexybacklog.Console("플레이씬 디스트로이");
        }

        private void OnApplicationPause(bool pause)
        {
            // 별다른 처리안해도 되는듯?
        }

    }

}