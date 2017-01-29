using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{

    public class MonsterSpriteMachine : MonoBehaviour, IDisposable
    {

        public event MonsterStateMachine.StateChangeHandler Action_changeEvent;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnActionFinished(string ActionStateID)
        {
            Action_changeEvent(this.name, ActionStateID);
        }

        public void Dispose()
        {
            Action_changeEvent = null;
        }
    }


}