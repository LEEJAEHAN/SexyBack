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
            gameInput = Singleton<GameInput>.getInstance();
            gameManager = Singleton<GameManager>.getInstance();

            Singleton<TableLoader>.getInstance().Init();


            // menu단계에서 이미 load되있어도, 스텟은 초기화후 다시계산한다.
            // 다른 global manager는 로드되지 않았을경우에만 로드하고, 로드할 파일이 없으면, 더미데이터를만든다.
            Singleton<MapManager>.getInstance().InitOrLoad();
            Singleton<TalentManager>.getInstance().InitOrLoad();
            Singleton<PremiumManager>.getInstance().InitOrLoad();
            Singleton<EquipmentManager>.getInstance().InitOrLoad();
            Singleton<PlayerStatus>.getInstance().Init();
            Singleton<PlayerStatus>.getInstance().ReCheckStat();

            gameManager.Init();
        }
        //GameModeData args;
        void Start()
        {            
            if (InstanceSaveSystem.InstanceXmlDataExist)
            {
                //try
                //{
                    sexybacklog.Console("인스턴스데이터 로드");
                    gameManager.LoadInstance();
                //}
                //catch (Exception e)
                //{
                //    sexybacklog.Console("로드에이상이있었습니다. 클리어합니다." + e.Message);
                //    InstanceSaveSystem.ClearInstance();
                //    OnDestroy();
                //    Debug.Break();
                //    SceneManager.LoadScene("MenuScene");
                //    return;
                //}
            }
            else // 메뉴로부터실행됬을때
            {
                sexybacklog.Console("새 인스턴스데이터 생성");
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
            // 주의 : 플레이 씬에서만 쓰이는 모든 singleton 해제가 필요하다.
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
            SaveSystem.SaveGlobalData();
        }
        private void OnApplicationPause(bool pause)
        {
            // 별다른 처리안해도 되는듯?
        }

    }

}