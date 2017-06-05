using UnityEngine;
using UnityEngine.SceneManagement;

namespace SexyBackRewardScene
{
    internal class MainReward : MonoBehaviour
    {
        RewardManager rManager;

        private void Awake()
        {
            Singleton<TableLoader>.getInstance().Init();

            if (Singleton<GlobalGameManager>.getInstance().LoadPlayerData() == false)
            {
                // 실패처리 ex) 디버그시 : 게임을 저장하지 않고 종료한다.
                // 실패처리 ex) 실제서비스시 : 복구가 필요한 시점. 저장하지않는다.
                sexybacklog.Console("실패처리. 디버그중지");
                Debug.Break();
            }
            Singleton<GlobalGameManager>.getInstance().ReBuildStat();

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
            SexyBackPlayScene.InstanceSaveSystem.ClearInstance();
            SceneManager.LoadScene("MenuScene");
        }
        private void OnDestroy()
        {
            // 주의 : 이 씬에서만 쓰이는 모든 singleton 해제가 필요하다.
            Singleton<RewardManager>.Clear();
        }
        private void OnApplicationQuit()
        {
            sexybacklog.Console("어플강제종료");
            // TODO : 종료할때 저장하는거 빼야한다.
            Singleton<GlobalGameManager>.getInstance().Dispose(); // 글로벌 데이터 세이브.
            Singleton<GlobalGameManager>.Clear(); // 글로벌 데이터 세이브. 
            Singleton<TableLoader>.Clear();
        }
    }
}