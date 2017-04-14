using UnityEngine;

namespace SexyBackPlayScene
{
    internal class MainScript : MonoBehaviour
    {
        double startTime = 1;
        double timer = 0;

        GameObject Window;
        private void Awake()
        {
            Window = GameObject.Find("RewardWindow");

            Window.SetActive(false);
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if(timer > startTime)
            {
                if(!Window.activeInHierarchy)
                {
                    Window.SetActive(true);
                    Window.GetComponent<UITweener>().PlayForward();
                }
            }


            
        }
    }
}