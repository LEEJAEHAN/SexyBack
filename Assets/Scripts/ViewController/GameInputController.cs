using UnityEngine;
using System.Collections;

namespace SexyBackPlayScene
{
    public class GameInputController : MonoBehaviour
    {
        public delegate void TapEvent_Handler();
        public event TapEvent_Handler noticeTap;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnTapDown(GameObject button)
        {
            
//            sexybacklog.Console(UICamera.currentTouch.pos); //ngui pos

            Vector2 position = UICamera.currentTouch.pos;

            position /= GameSetting.pixelPerUnit;

            position.x -= (GameSetting.aspect * GameSetting.orthographicSize);
            position.y -= (GameSetting.orthographicSize);

            // step1 : nguiposition /= pixerperunit(==100)
            // spet2 :  x -= (aspect * orthographicSize)(==3.2) , y -= orthographicSize(==6.4) (field( x -= 3.6, y -= 6.4 )


            //            sexybacklog.Console(position); //game pos 

            ViewLoader.hitparticle.transform.position = position;
            ViewLoader.hitparticle.GetComponent<ParticleSystem>().Play();
            noticeTap();
        }
    }

}