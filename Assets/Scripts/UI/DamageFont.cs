using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SexyBackPlayScene
{
    public class DamageFont : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }
        

        // 여기 얹혀서 처리 ㅠ
        public void OnDamageFontFinish()
        {
            // 이것만 직접처리한다. 귀찮엉...
            this.gameObject.SetActive(false);
        }
    }
}