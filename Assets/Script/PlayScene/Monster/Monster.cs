using System;
using UnityEngine;
using System.Runtime.Serialization;

namespace SexyBackPlayScene
{
    [Serializable]
    internal class Monster : IStateOwner, IDisposable, ISerializable// model
    {
        ~Monster() { sexybacklog.Console("몬스터 소멸!"); }

        public readonly string ID;
        public readonly string DataID;
        public int level;
        public BigInteger HP;
        public string Name;
        public bool isBoss;
        public BigInteger MAXHP;
        // view
        public GameObject avatar;
        public GameObject sprite;
        // state
        public StateMachine<Monster> StateMachine;
        //size value
        public Vector3 CenterPosition; // 몬스터 중점의 world상 위치.
        public Vector3 Size;           // sprite size, collider size는 이것과 동기화.

        public string CurrentState { get { return StateMachine.currStateID; } }
        public MonsterStateMachine.StateChangeHandler Action_StateChangeEvent { set { StateMachine.Action_changeEvent += value; } }

        public string GetID { get { return ID; } }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", ID);
            info.AddValue("DataID", DataID);
            info.AddValue("level", level);
            info.AddValue("HP", HP.ToString());
        }
        public Monster(SerializationInfo info, StreamingContext context)
        {
            ID = (string)info.GetValue("ID", typeof(string));
            DataID = (string)info.GetValue("DataID", typeof(string));
            level = (int)info.GetValue("level", typeof(int));
            HP = new BigInteger((string)info.GetValue("HP", typeof(string)));
        }

        internal Monster(string id, string dataID)
        {
            ID = id;
            DataID = dataID;
        }

        internal void Spawn(Transform transform) // join the battle
        {
            avatar.transform.parent = transform;
            avatar.transform.localPosition = Vector3.zero;
            avatar.SetActive(true);
            StateMachine.ChangeState("Appear");
        }   

        public void Update()
        {
            StateMachine.Update();
        }

        internal bool Hit(Vector3 hitWoridPosition, BigInteger damage, bool isCritical)
        {
            HP -= damage;
            Singleton<InstanceStatus>.getInstance().ExpGain(damage, true);
            //sexybacklog.Console(damage);
            //particle
            EffectController.getInstance.PlayParticle(hitWoridPosition);
            //damagefont

            Vector3 hitScreenPosition = GameCameras.HeroCamera.WorldToScreenPoint(hitWoridPosition);
            
            if(GameCameras.DamageFontFlag)
                EffectController.getInstance.PlayDamageFont(damage, hitScreenPosition);

            //avatar
            sprite.GetComponent<Animator>().rootPosition = avatar.transform.position;
            if (isCritical)
                sprite.GetComponent<Animator>().SetTrigger("Hit_Critical");
            else
                sprite.GetComponent<Animator>().SetTrigger("Hit");

            if (HP < 0)
                HP = 0;

            Singleton<MonsterManager>.getInstance().onHit(this);

            if (HP <= 0) // dead check
            {
                if (StateMachine.currStateID == "Ready")
                    StateMachine.ChangeState("Flying");
                return false; // will be destroyed;
            }
            else
                return true;
        }

        public void Dispose()
        {
            HP = null;
            MAXHP = null;
            if(avatar!= null) // stage에 의해 먼저 제거될 위험이 있음.
                avatar.GetComponent<MonsterView>().Dispose();
            avatar = null;
            sprite = null;
            StateMachine = null;
        }

    }
}