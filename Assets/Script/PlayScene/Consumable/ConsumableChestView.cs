using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SexyBackPlayScene
{
    public class ConsumableChestView : MonoBehaviour
    {
        double TimeOutTime = 5f;
        double Timer = 0;
        bool Flash = false;
        bool Open = false;

        public Action Action_Open;
        public Action Action_Destroy;

        public void Awake()
        {
        }

        public void Start()
        {
            //transform.OverlayPosition(ViewLoader.monsterbucket.transform.position, GameCameras.HeroCamera, GameCameras.UICamera);
            //transform.position = new Vector3(transform.position.x, transform.position.y, 0f );
            
            GetComponent<TweenPosition>().from = transform.localPosition;
            //GetComponent<TweenPosition>().ResetToBeginning();
            UIWidget screen = transform.parent.gameObject.GetComponent<UIWidget>();
            int Top = screen.height / 2;
            int Bottom = - screen.height / 2;
            int Left = - screen.width / 2;
            int Right = screen.width / 2;

            int xExpend = GetComponent<UIWidget>().width / 2;
            int TopExpend = GetComponent<UIWidget>().height * 3 / 2;
            int BottomExpend = GetComponent<UIWidget>().height / 2;

            int x = UnityEngine.Random.Range(Left + xExpend, Right - xExpend);
            int y = UnityEngine.Random.Range(Top - TopExpend, Bottom + BottomExpend);
            GetComponent<TweenPosition>().to = new Vector3(x, y, 0);
            //GetComponent<TweenPosition>().to = new Vector3(Right - xExpend, Bottom + BottomExpend, 0);
            //GetComponent<TweenPosition>().to = new Vector3(Left + xExpend, Top - TopExpend, 0);

            GetComponent<TweenPosition>().PlayForward();


            transform.GetChild(0).gameObject.SetActive(false);

        }
        public void onTweenComplete()
        {
            //TimeOutTime = 5f;
            Timer = TimeOutTime;
            //GetComponent<BoxCollider>().enabled = true;
        }
        public void onEquipTweenComplete()
        {
            gameObject.SetActive(false);
            Action_Destroy();
        }
        public void onOpen()
        {
            if (Open)
                return;
            GetComponent<UISprite>().spriteName = "Icon_30";
            Open = true;
            GetComponent<TweenAlpha>().ResetToBeginning();
            GetComponent<TweenAlpha>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(true);
            GetComponent<TweenPosition>().PlayForward();
            Action_Open();
        }

        // Update is called once per frame
        void Update()
        {
            if (Timer <= 0)
                return;

            Timer -= Time.deltaTime;
            if (Timer <= TimeOutTime / 2 && !Flash && !Open)
            {
                Flash = true;
                GetComponent<TweenAlpha>().PlayForward();
            }
            if (Timer <= 0 && !Open)
            {
                gameObject.SetActive(false);
                Action_Destroy();
            }
        }
    }
}
