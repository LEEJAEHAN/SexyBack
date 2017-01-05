using UnityEngine;
using System.Collections;

namespace SexyBackPlayScene
{
    public class MonsterAvatar : MonoBehaviour
    {

        public SexyBackMonster monster;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Projectile")
            {
                //                double damage = source.gameObject.GetComponent<Projectile>().damage;

                double damage = 10;
                monster.Hit(damage, true);
            }

        }
    }

}
