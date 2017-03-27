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
            InitUISetting();
        }

        private void InitUISetting()
        {
            GameObject.Find("UI PopUp").transform.position = GameObject.Find("UI Root").transform.position;
            GameObject.Find("UI PopUp").SetActive(false);
            GameObject.Find("Middle_Window").SetActive(false);
            GameObject.Find("Slot1New").SetActive(false);
            GameObject.Find("Slot2New").SetActive(false);
            GameObject.Find("SP").GetComponent<UILabel>().text ="";
            GameObject.Find("GEM").GetComponent<UILabel>().text = "";
            GameObject.Find("IconTable").transform.DestroyChildren();
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
            Debug.Log("메뉴신 소멸");
        }
    }

}
