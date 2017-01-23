using UnityEngine;
using System.Collections;

namespace SexyBackPlayScene
{
    public class MonsterView : MonoBehaviour
    {
        // event publisher
        public delegate void MonsterHit_Event(string monsterID, Vector3 hitPosition, BigInteger damage, bool isCritical);
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
                Projectile proj = collider.gameObject.GetComponent<Projectile>();


                noticeHit(this.name, collider.transform.position, proj.Damage, false);
            }
        }

        public void OnDamageFontFinish()
        {
            // 이것만 직접처리한다. 귀찮엉...
            ViewLoader.DamageFont.SetActive(false);
        }
    }

}
