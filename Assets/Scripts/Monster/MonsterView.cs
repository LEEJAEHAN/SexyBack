﻿using UnityEngine;
using System.Collections;

namespace SexyBackPlayScene
{
    public class MonsterView : MonoBehaviour
    {
        // event publisher
        public delegate void MonsterHit_Event(string monsterID, Projectile projectile);
        public event MonsterHit_Event noticeHit;

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
                noticeHit(this.name, collider.gameObject.GetComponent<Projectile>());
            }
        }
    }

}
