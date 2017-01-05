using UnityEngine;
using System.Collections;

namespace SexyBackPlayScene
{
    public class Slash : MonoBehaviour
    {
        float r;
        float g;
        float b;
        float alpha;

        public Vector3 NormalPosition;
        public Vector3 YX_flipPosition;
        public Vector3 Y_filpPosition;

        bool flipx = false;
        bool flipy = false;
        int xVec = 1;
        int yVec = 1;
        double ActionTime = 0.5;

        Color[] colorbucket = new Color[6];
        
        // Use this for initialization
        void Start()
        {
            float b = 0.15f;

            colorbucket[0] = new Color(1f, b, b, 1f);
            colorbucket[1] = new Color(b, 1f, b, 1f);
            colorbucket[2] = new Color(b, b, 1f, 1f);
            colorbucket[3] = new Color(1f, 1f, b, 1f);
            colorbucket[4] = new Color(1f, b, 1f, 1f);
            colorbucket[5] = new Color(b, 1f, 1f, 1f);
        }

        // Update is called once per frame
        void Update()
        {
            float deltaTime = Time.deltaTime;
            ActionTime -= deltaTime;
            if(ActionTime > 0)
                transform.Translate(new Vector3(deltaTime * xVec, deltaTime * yVec, 0));

        }


        public void Play()
        {
            GameManager.SexyBackLog("change color");

            flipx = (Random.Range(1, 10) >= 5f);
            GetComponent<SpriteRenderer>().flipX = flipx;
            flipy = (Random.Range(1, 10) >= 5f);
            GetComponent<SpriteRenderer>().flipY = flipy;
            if(flipy)
            {
                if(flipx)
                    transform.position = YX_flipPosition;
                else
                    transform.position = Y_filpPosition;
            }
            else
            {
                transform.position = NormalPosition;
            }


            if (flipx)
                xVec = -1;
            else
                xVec = 1;
            if (flipy)
                yVec = -1;
            else
                yVec = 1;


            ActionTime = 0.3;
            int index = Random.Range(0, 5);
            GetComponent<SpriteRenderer>().color = colorbucket[index];
            GetComponent<Animator>().SetTrigger("Play");

        }
    }
}