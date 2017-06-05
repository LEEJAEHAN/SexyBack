using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SexyBackMenuScene
{
    public class MainMenu : MonoBehaviour
    {
        private void Awake()
        {
            Singleton<TableLoader>.getInstance().Init();
            // TODO :  ResourceLoader 제작, 캐싱;

            // 파일이없을시에만 이닛. 있으면 무조건 로드시도, 실패시 메시지처리
            if (SexyBackPlayScene.InstanceSaveSystem.InstanceXmlDataExist) // case PlayScene
            {
                sexybacklog.Console("인스턴스 데이터를 불러옵니다.");
                SceneManager.LoadScene("PlayScene");
                return;
            }

            sexybacklog.Console("진행중인 인스턴스 저장데이터가 없습니다.");
            if (Singleton<GlobalGameManager>.getInstance().LoadPlayerData() == false)
            {
                // 실패처리 ex) 디버그시 : 게임을 저장하지 않고 종료한다.
                // 실패처리 ex) 실제서비스시 : 복구가 필요한 시점. 저장하지않는다.
                sexybacklog.Console("실패처리. 디버그중지");
                Debug.Break();
            }

            Singleton<GlobalGameManager>.getInstance().ReBuildStat();
            Singleton<ViewLoader>.getInstance().InitUISetting();
        }

        private void Start()
        {
        }

        public void QuitGame()
        {
            Application.Quit();
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
        private void OnDestroy()
        {
            Singleton<ViewLoader>.Clear();
            sexybacklog.Console("메뉴신 소멸");
        }
    }

}
