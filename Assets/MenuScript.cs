using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SexyBackMenuScene
{

    public class MenuScript : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void NewGame()
        {
            SceneManager.LoadScene("BattleScene");
        }

        public void LoadGame()
        {

            if (PlayerPrefs.HasKey("게임중저장데이터"))
            {
                string data = PlayerPrefs.GetString("게임중저장데이터");
                Debug.Log(data + "를 불러옵니다");
                SceneManager.LoadScene("BattleScene");
            }
            else
                Debug.Log("저장데이터가음습니다.");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void OnDisable()
        {
            Debug.Log("디저블");
        }

        private void OnDestroy()
        {
            Debug.Log("디스트로이");
        }
    }

}
