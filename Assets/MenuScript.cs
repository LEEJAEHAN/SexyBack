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

        public void onClick()
        {
            SceneManager.LoadScene("BattleScene");
            Debug.Log("0nclick");
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
