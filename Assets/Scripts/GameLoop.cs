using UnityEngine;
using System.Collections;

namespace SexyBackPlayScene
{
    public class GameLoop : MonoBehaviour
    {
        GameManager gameManager;

        
        // Use this for initialization
        void Start()
        {
            gameManager = GameManager.getInstance();
            gameManager.Init();
        }

        // Update is called once per frame
        void Update()
        {
            gameManager.Update();

        }

        public void OnTap(GameObject sender)
        {

            if (sender.name == "button_tap")
            {
                gameManager.Tap();
                GameManager.SexyBackLog("tap"); 
            }
            //
            gameManager.noticeEvent(sender);
        }
    }

}