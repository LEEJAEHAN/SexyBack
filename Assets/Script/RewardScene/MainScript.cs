using UnityEngine;

namespace SexyBackRewardScene
{
    internal class MainScript : MonoBehaviour
    {
        double startTime = 1;
        double timer = 0;
        double appearTick = 0.3f;
        bool pause = false;

        private void Awake()
        {
            Singleton<RewardManager>.getInstance().InitWindow();
        }

        private void Update()
        {
            if(!pause)
                timer += Time.deltaTime;

            if(timer > startTime)
            {
                Singleton<RewardManager>.getInstance().ActiveWindow();

                if(timer > startTime + 1f + appearTick)
                {
                    if (!Singleton<RewardManager>.getInstance().NextShow())
                    {
                        timer -= appearTick;
                    }
                    else
                    {
                        pause = true;
                    }
                }
            }
        }
    }
}