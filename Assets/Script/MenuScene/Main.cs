﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SexyBackMenuScene
{
    public class Main : MonoBehaviour
    {
        bool LoadComplete = false;

        private void Awake()
        {
            Singleton<TableLoader>.getInstance().Init();
            // TODO :  ResourceLoader 제작, 캐싱;

            // 파일이없을시에만 이닛. 있으면 무조건 로드시도, 실패시 메시지처리
            // set scene state
            if (SexyBackPlayScene.InstanceSaveSystem.InstanceXmlDataExist) // case PlayScene
            {
                sexybacklog.Console("인스턴스 데이터를 불러옵니다.");
                SceneManager.LoadScene("PlayScene");
            }
            else // case MenuScene
            {
                sexybacklog.Console("진행중인 인스턴스 저장데이터가 없습니다.");

                // init or load
                Singleton<EquipmentManager>.getInstance().InitOrLoad();
                Singleton<TalentManager>.getInstance().InitOrLoad();
                Singleton<PremiumManager>.getInstance().InitOrLoad();
                Singleton<MapManager>.getInstance().InitOrLoad();

                Singleton<PlayerStatus>.getInstance().Init();
                Singleton<PlayerStatus>.getInstance().ReCheckStat();

                Singleton<ViewLoader>.getInstance().InitUISetting();
            }

            LoadComplete = true;
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
            if(LoadComplete)
                SaveSystem.SaveGlobalData(); // 글로벌 데이터 세이브.
        }
        private void OnDestroy()
        {
            Singleton<ViewLoader>.Clear();
            sexybacklog.Console("메뉴신 소멸");
        }
    }

}
