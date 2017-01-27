using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class BackCollision : MonoBehaviour
    {
        // event publisher
        public static event MonsterView.MonsterHit_Event Action_HitEvent = delegate { };

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
        public void OnDamageFontFinish()
        {
            // 이것만 직접처리한다. 귀찮엉...
            ViewLoader.DamageFont.SetActive(false);
        }
    }
}