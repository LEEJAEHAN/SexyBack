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
            if (collider.gameObject.tag == "Wall")
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
            GetComponent<Rigidbody>().velocity = new Vector3(3, 3, 0);
            GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 10);
        }
        internal void Flip()
        {
            //Vector3 prevRot = GetComponent<Rigidbody>().angularVelocity;
            //GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, prevRot.z);
            //GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 10);
            sexybacklog.Console("Flip!");
        }

    }

}
