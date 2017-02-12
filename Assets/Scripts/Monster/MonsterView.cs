using UnityEngine;
using System.Collections;
using System;
namespace SexyBackPlayScene
{
    public class MonsterView : MonoBehaviour, IDisposable
    {
        // event publisher
        public delegate void MonsterHit_Event(Vector3 hitPosition, string elementID);
        public event MonsterHit_Event Action_HitEvent = delegate { };
        
        bool isdisposing = false;
        int WallHitCount = 7;

        // Use this for initialization
        void Start()
        {

        }
        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider collider)
        {
            if (isdisposing)
                return;
            if (collider.gameObject.tag == "Projectile")
            {
                Action_HitEvent(collider.transform.position, collider.gameObject.name);
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Wall")
            {
                Flip();
            }
        }

        public void Dispose()
        {
            isdisposing = true;
            Action_HitEvent = null;
            Destroy(this.gameObject);
        }

        internal void Fly()
        {
            GetComponent<Rigidbody>().isKinematic = false;

            float randx = UnityEngine.Random.Range(-5f, 5f);
            if (randx < 0 && randx > -2)
                randx -= 2;
            else if (randx >= 0 && randx < 2)
                randx += 2;

            Vector3 force = new Vector3(randx, UnityEngine.Random.Range(2f, 5f),1.25f);
            force = force.normalized;

            GetComponent<BoxCollider>().size = Vector3.one;
            GetComponent<Rigidbody>().AddForce(force * 1250);
            GetComponent<Rigidbody>().maxAngularVelocity = 50;
            GetComponent<Rigidbody>().AddTorque(0, 0, UnityEngine.Random.Range(-100,100));
        }
        internal void Flip()
        {
            WallHitCount--;
            if (WallHitCount <= 0)
                GetComponent<BoxCollider>().enabled = false;

            GetComponent<Rigidbody>().AddTorque(0, 0, UnityEngine.Random.Range(-100, 100));

            //            GetComponent<Rigidbody>().AddForce( = new Vector3(10f, 6f, 0);
        }

    }

}
