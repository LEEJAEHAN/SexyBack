using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    public class EffectController : MonoBehaviour
    {
        GameObject Effect_Particle;
        GameObject Effect_Sword;
        GameObject Effect_Buff;

        Queue<GridItemIcon> BuffEffectsQueue = new Queue<GridItemIcon>();

        private static EffectController instance;
        public static EffectController getInstance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.Find("Effect").AddComponent<EffectController>();

                return instance;
            }
        }
        //public void OnApplicationQuit()
        //{
        //    instance = null;
        //}

        // Use this for initialization
        private void Awake()
        {   // init
            Effect_Particle = GameObject.Find("Effect_HitParticle");
            Effect_Sword = GameObject.Find("Effect_Sword");

            Effect_Buff = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/EffectBuff"));
            Effect_Buff.transform.parent = this.gameObject.transform;
            Effect_Buff.transform.localScale = this.gameObject.transform.localScale;
            Effect_Buff.GetComponent<UITweener>().ResetToBeginning();
            EventDelegate eventdel = new EventDelegate(this, "onBuffEffectFinish");
            Effect_Buff.GetComponent<TweenPosition>().AddOnFinished(eventdel);
            Effect_Buff.SetActive(false);
        }

        public void OnDamageFontFinish(GameObject damageFontEffect)
        {
            GameObject.Destroy(damageFontEffect);
        }

        internal void PlayDamageFont(BigInteger dmg, Vector3 position)
        {
            Vector3 screenpos = ViewLoader.HeroCamera.WorldToScreenPoint(position);
            GameObject Effect_DamageFont = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/UI/EffectDamageFont"));
            Effect_DamageFont.transform.parent = this.gameObject.transform;
            Effect_DamageFont.transform.localPosition = screenpos;

            Effect_DamageFont.SetActive(true);
            Effect_DamageFont.GetComponent<UILabel>().text = dmg.To5String();
            Effect_DamageFont.GetComponent<UILabel>().fontSize = (int)((30 + 10 * (10 - screenpos.z)));

            EventDelegate eventdel = new EventDelegate(this, "OnDamageFontFinish");
            eventdel.parameters[0] = new EventDelegate.Parameter();
            eventdel.parameters[0].obj = Effect_DamageFont;
            eventdel.parameters[0].expectedType = typeof(GameObject);

            Effect_DamageFont.GetComponent<TweenScale>().AddOnFinished(eventdel);
        }

        internal void PlayParticle(Vector3 position)
        {
            Effect_Particle.transform.position = position;
            Effect_Particle.GetComponent<ParticleSystem>().Play();
        }

        internal void PlaySwordEffect(Vector3 effectPos, Vector3 rotVector, bool isCritical)
        {
            Effect_Sword.transform.eulerAngles = rotVector;

            // translation
            Effect_Sword.transform.position = effectPos;

            // play
            if (isCritical)
                Effect_Sword.GetComponent<Animator>().SetTrigger("Play_Critical");
            else
                Effect_Sword.GetComponent<Animator>().SetTrigger("Play");
            //sexybacklog.Console("Hit:"+SwordEffect.transform.position);
        }

        void onBuffEffectFinish()
        {
            Effect_Buff.GetComponent<UITweener>().ResetToBeginning();
            Effect_Buff.SetActive(false);
            if (BuffEffectsQueue.Count > 0)
                PlayBuffEffect(BuffEffectsQueue.Dequeue());
        }
        internal void AddBuffEffect(GridItemIcon icon)
        {
            BuffEffectsQueue.Enqueue(icon);
            if (Effect_Buff.GetComponent<UITweener>().isActiveAndEnabled)
                return;
            else
                PlayBuffEffect(BuffEffectsQueue.Dequeue());
        }
        void PlayBuffEffect(GridItemIcon icon)
        {
            Effect_Buff.SetActive(true);
            GridItemIcon.Draw(icon, Effect_Buff);
            Effect_Buff.GetComponent<UITweener>().PlayForward();
        }

    }
}
