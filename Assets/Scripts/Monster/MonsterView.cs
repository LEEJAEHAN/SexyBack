﻿using UnityEngine;
using System.Collections;

namespace SexyBackPlayScene
{
    public class MonsterView : MonoBehaviour
    {
        // event publisher
        public delegate void MonsterHit_Event(Vector3 hitPosition, string elementID);
        public event MonsterHit_Event Action_HitEvent = delegate { };

        public event MonsterStateMachine.StateChangeHandler Action_changeEvent;

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
            if (collider.gameObject.tag == "Projectile")
            {
                Action_HitEvent(collider.transform.position, collider.gameObject.name);
            }
        }

        public void OnActionFinished(string ActionStateID)
        {
            Action_changeEvent(this.name, ActionStateID);
        } 
        public void OnDamageFontFinish()
        {
            // 이것만 직접처리한다. 귀찮엉...
            ViewLoader.DamageFont.SetActive(false);
        }
    }

}
