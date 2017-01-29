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

        public event MonsterStateMachine.StateChangeHandler Action_changeEvent;


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
        }

        public void OnActionFinished(string ActionStateID)
        {
            Action_changeEvent(this.name, ActionStateID);
        } 

        public void Dispose()
        {
            isdisposing = true;
            Action_HitEvent = null;
            Action_changeEvent = null;
            Stop();
            Destroy(this.gameObject);
        }

        private void Stop()
        {
            transform.parent.position = GameSetting.ECamPosition;
            transform.parent.rotation = Quaternion.Euler(0, 0, 0);
            transform.parent.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.parent.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        internal void Fly()
        {
            transform.parent.GetComponent<Rigidbody>().velocity = new Vector3(2, 2, 0);
            transform.parent.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 5);
        }

    }

}
