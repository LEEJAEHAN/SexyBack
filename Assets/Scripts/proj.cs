using UnityEngine;
using System.Collections;

namespace SexyBackPlayScene
{

    public class proj : MonoBehaviour
    {

        public Transform target;
        public Transform source;

//        public Transform throwPoint;


        public GameObject ball;


        void Start()
        {
        //    Throw();
        }


        void Update()
        {
            float xVec = GetComponent<Rigidbody>().velocity.x;
            float yVec = GetComponent<Rigidbody>().velocity.y;

            float rad = Mathf.Atan2(yVec, xVec);

            float rot = UnityEngine.Mathf.Atan2(yVec, xVec) * UnityEngine.Mathf.Rad2Deg;

            GameManager.SexyBackDebug(rot + " " + xVec + " " + yVec);

            transform.eulerAngles = new Vector3(0, 0, rot + 90);
        }

        public void Throw()
        {
            transform.position = source.position;
            transform.localScale = source.localScale;
            Physics.gravity = new Vector3(0,-20.0f,0);

            GetComponent<Rigidbody>().useGravity = true;
            float xDistance;
            float yDistance;
            float zDistance;

            xDistance = target.transform.position.x - source.position.x;
            yDistance = target.transform.position.y - source.position.y;
            zDistance = target.transform.position.z - source.position.z;

            // yz 앵글이랑
            // xy 앵글
            float throwangle_xy;
            
            throwangle_xy = Mathf.Atan((yDistance + (-Physics.gravity.y/2)) / xDistance);

            float totalVelo =  xDistance / Mathf.Cos(throwangle_xy);

            float xVelo, yVelo;
            xVelo = xDistance;
            yVelo = xDistance * Mathf.Tan(throwangle_xy);
            float zVelo = zDistance;

            GetComponent<Rigidbody>().velocity= new Vector3(xVelo,yVelo, zVelo);
        }
    }

}