using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{


    public class OptionView : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GiveUpGame()
        {
            this.gameObject.SetActive(false);

            string context = "[000000]클리어 보상의 일부만 받을 수 있습니다.\n\n게임을 그만두고\n메인화면으로 이동하시겠습니까[-]";

            ViewLoader.MakePopUp("던전포기", context, GiveUpYes, GiveUpNo);
        }
        private void GiveUpYes()
        {
            Singleton<InstanceGameManager>.getInstance().EndGame(false);
        }
        private void GiveUpNo()
        {
            this.gameObject.SetActive(true);
        }

        public void ExitGame()
        {
            sexybacklog.Console("어플종료클릭, 명시적 세이브");
            //Singleton<InstanceGameManager>.getInstance().SaveInstance();
            Application.Quit();
        }
        public void ExitOption()
        {

        }
        public void ToggleSound()
        {

        }
        public void ToggleVibe()
        {

        }

    }
}