using UnityEngine;
using System.Collections;

namespace SexyBackPlayScene
{
    public class MonsterView : MonoBehaviour
    {
        // event publisher
        public delegate void MonsterHit_Event(string monsterID, Vector3 hitPosition, string elementID);
        public event MonsterHit_Event noticeHit;


        public delegate void MonstserActionEndEvent(string monsterID, string stateID);
        public event MonstserActionEndEvent noticeStateEnd;


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

        public void OnActionFinished(string ActionStateID)
        {
            noticeStateEnd(this.name, ActionStateID);
        } 
        public void OnDamageFontFinish()
        {
            // 이것만 직접처리한다. 귀찮엉...
            ViewLoader.DamageFont.SetActive(false);
        }
    }

}
