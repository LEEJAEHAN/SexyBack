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
            Singleton<TableLoader>.getInstance().Init();

            if (SaveSystem.CanLoad())
            {
                Debug.Log("인스턴스데이터를 불러옵니다.");
                SceneManager.LoadScene("PlayScene");
            }
            else
                Debug.Log("저장데이터가음습니다.");

            Singleton<ViewLoader>.getInstance().InitUISetting();
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
            if (SaveSystem.CanLoad())
            {
                Debug.Log("인스턴스데이터를 불러옵니다.");
                SceneManager.LoadScene("PlayScene");
            }
            else
                Debug.Log("저장데이터가음습니다.");
            //PlayerPrefs.HasKey("InstanceData")
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
