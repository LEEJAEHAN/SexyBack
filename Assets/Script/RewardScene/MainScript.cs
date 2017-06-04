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

            Singleton<MapManager>.getInstance().InitOrLoad();
            Singleton<TalentManager>.getInstance().InitOrLoad();
            Singleton<PremiumManager>.getInstance().InitOrLoad();
            Singleton<EquipmentManager>.getInstance().InitOrLoad();
            Singleton<PlayerStatus>.getInstance().Init();
            Singleton<PlayerStatus>.getInstance().ReCheckStat();


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
            SexyBackPlayScene.InstanceSaveSystem.ClearInstance();
            SceneManager.LoadScene("MenuScene");
        }
    }
}