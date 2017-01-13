using UnityEngine;
using System.Collections;

namespace SexyBackPlayScene
{
    public class MonsterBehaviour : MonoBehaviour
    {
        // 뷰가 모델을 참조한다
        public SexyBackMonster monster;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        // oncollision, ontrigger 을 delegate로 붙일수 없어서 임시방편, 이것만 view에있다.
        // 실제론 monster클래스에서 날려야하는데, 물리엔진을 유니티영역의것을 쓰다보니,  monster controller를 따른다.
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Projectile")
                monster.OnHit(collider.gameObject.GetComponent<Projectile>().Damage);


        }
    }

}
