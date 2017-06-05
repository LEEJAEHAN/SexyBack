using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

namespace SexyBackPlayScene
{
    public class MainPlay : MonoBehaviour
    {
        ViewLoader viewLoader;
        InstanceGameManager instanceGameManager;
        GameInput gameInput;
        GameCameras cameraSetting;

        // Use this for initialization
        void Awake()
        {
            viewLoader = new ViewLoader();
            cameraSetting = new GameCameras();

            gameInput = Singleton<GameInput>.getInstance();
            instanceGameManager = Singleton<InstanceGameManager>.getInstance();

            Singleton<TableLoader>.getInstance().Init();

            // menu단계에서 이미 load되있어도, 스텟은 초기화후 다시계산한다.
            // 다른 global manager는 로드되지 않았을경우에만 로드하고, 로드할 파일이 없으면, 더미데이터를만든다.
            if (Singleton<GlobalGameManager>.getInstance().LoadPlayerData() == false)
            {
                // 실패처리 ex) 디버그시 : 게임을 저장하지 않고 종료한다.
                // 실패처리 ex) 실제서비스시 : 복구가 필요한 시점. 저장하지않는다.
                sexybacklog.Console("실패처리. 디버그중지");
                Debug.Break();
            }
            Singleton<GlobalGameManager>.getInstance().ReBuildStat();

            instanceGameManager.Init();
        }
        //GameModeData args;
        void Start()
        {            
            if (InstanceSaveSystem.InstanceXmlDataExist)
            {
                sexybacklog.Console("인스턴스데이터 로드");
                if(instanceGameManager.LoadInstanceData() == false)
                {
                    // 실패처리 ex) 디버그시 : 게임을 저장하지 않고 종료한다.
                    // 실패처리 ex) 실제서비스시 : 인스턴스데이터를 클리어하고 메뉴로돌아간다.
                    sexybacklog.Console("실패처리. 디버그중지");
                    Debug.Break();
                }
            }
            else // 메뉴로부터실행됬을때
            {
                sexybacklog.Console("새 인스턴스데이터 생성");
                instanceGameManager.NewInstance();
            }
        }
        // Update is called once per frame
        void Update()
        {
            instanceGameManager.Update();
            gameInput.CheckInput();
        }
        private void FixedUpdate()
        {
            instanceGameManager.FixedUpdate();
        }
        public void Option()
        {
            instanceGameManager.Pause(true);
            ViewLoader.PopUpPanel.SetActive(true);
            ViewLoader.OptionPanel.SetActive(true);
        }
        public void ExitOption()
        {
            instanceGameManager.Pause(false);
            ViewLoader.OptionPanel.SetActive(false);
            ViewLoader.PopUpPanel.SetActive(false);
        }

        private void OnDestroy()    
        {
            // 주의 : 플레이 씬에서만 쓰이는 모든 singleton 해제가 필요하다.
            sexybacklog.Console("플레이씬 디스트로이");
            gameInput.Dispose();
            instanceGameManager.Dispose();
            Singleton<GameInput>.Clear();
            Singleton<InstanceGameManager>.Clear();
            Singleton<ViewLoader>.Clear();
        }
        private void OnApplicationQuit()
        {
            sexybacklog.Console("어플강제종료");
            // TODO : 종료할때 저장하는거 빼야한다.
            Singleton<GlobalGameManager>.getInstance().SaveGlobalData(); // 글로벌 데이터 세이브. 
            Singleton<GlobalGameManager>.getInstance().Dispose(); // 글로벌 데이터 세이브.
            Singleton<GlobalGameManager>.Clear(); // 글로벌 데이터 세이브. 
            Singleton<TableLoader>.Clear();
        }
        private void OnApplicationPause(bool pause)
        {
            // 별다른 처리안해도 되는듯?
        }

    }

}