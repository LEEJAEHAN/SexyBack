using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SexyBackMenuScene
{
    public class MenuScript : MonoBehaviour
    {

        private void Awake()
        {
            Singleton<ViewLoader>.getInstance().InitUISetting();
            Singleton<TableLoader>.getInstance().Init();
        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TextNewGame()
        {
            SceneManager.LoadScene("PlayScene");
        }

        public void TestLoadGAme()
        {
            if (PlayerPrefs.HasKey("InstanceData"))
            {
                Debug.Log("인스턴스데이터를 불러옵니다.");
                SceneManager.LoadScene("PlayScene");
            }
            else
                Debug.Log("저장데이터가음습니다.");
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
