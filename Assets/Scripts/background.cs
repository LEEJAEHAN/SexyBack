using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class background : MonoBehaviour
    {
        // event publisher
        public event MonsterView.MonsterHit_Event noticeHit;

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
                noticeHit(this.name, collider.transform.position, collider.gameObject.name);
            }
        }
    }
}