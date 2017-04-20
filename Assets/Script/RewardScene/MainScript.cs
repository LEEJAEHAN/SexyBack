using UnityEngine;
using UnityEngine.SceneManagement;

namespace SexyBackRewardScene
{
    internal class MainScript : MonoBehaviour
    {
        RewardManager rManager;
        bool pause = false;
        bool firstTouch = false;

        private void Awake()
        {
            rManager = Singleton<RewardManager>.getInstance();
            rManager.Init();
            rManager.InitWindow();
        }

        private void Update()
        {
            if(!pause)
                rManager.Update();
        }

        public void onFinishTween()
        {
            rManager.TweenComplete();
        }

        public void onTouch()
        {
            Debug.Log("클릭함");
            if (rManager.ShowAll())
                pause = true;
        }
        public void onButtonClick()
        {
            pause = true;
            rManager.Dispose();
            Singleton<RewardManager>.Clear();
            SceneManager.LoadScene("MenuScene");
        }
    }
}