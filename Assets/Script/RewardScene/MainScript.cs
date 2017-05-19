using UnityEngine;
using UnityEngine.SceneManagement;

namespace SexyBackRewardScene
{
    internal class MainScript : MonoBehaviour
    {
        RewardManager rManager;

        private void Awake()
        {
            Singleton<TableLoader>.getInstance().Init();
            Singleton<PlayerStatus>.getInstance().Init();
            Singleton<EquipmentManager>.getInstance().Init();

            rManager = Singleton<RewardManager>.getInstance();
            rManager.Init();
        }

        private void Update()
        {
            rManager.Update();
        }

        public void onFinishTween()
        {
            rManager.TweenComplete();
        }
        public void onTouch()
        {
            rManager.ShowNext();
        }
        public void onButtonClick()
        {
            Singleton<RewardManager>.Clear();
            SceneManager.LoadScene("MenuScene");
        }
    }
}