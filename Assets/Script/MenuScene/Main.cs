﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SexyBackPlayScene;

namespace SexyBackMenuScene
{
    public class Main : MonoBehaviour
    {
        private void Awake()
        {
            Singleton<TableLoader>.getInstance().Init();
            // TODO :  ResourceLoader 제작, 캐싱;

            if (SaveSystem.CanLoad())
            {
                Debug.Log("인스턴스데이터를 불러옵니다.");
                SceneManager.LoadScene("PlayScene");
            }
            else
                Debug.Log("저장데이터가음습니다.");

            Singleton<ViewLoader>.getInstance().InitUISetting();
        }

        internal void GoPlayScene(string selectedMapID, bool selectedBonus)
        {
            Singleton<StageManager>.getInstance().Init(selectedMapID, selectedBonus);
            SceneManager.LoadScene("PlayScene");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void OnDestroy()
        {
            Singleton<ViewLoader>.Clear();
            Debug.Log("메뉴신 소멸");
        }
    }

}