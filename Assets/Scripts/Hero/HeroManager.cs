using System;
using UnityEngine;

namespace SexyBackPlayScene
{
    class HeroManager
    {
        SexyBackHero Hero;

        double gameTime;
        public double testTimeTick = 1;

        UILabel label_herodmg = ViewLoader.label_herodmg.GetComponent<UILabel>();
        UILabel label_exp = ViewLoader.label_exp.GetComponent<UILabel>();

        internal void Init()
        {
            Hero = new SexyBackHero();
        }

        internal void OnKeyEvent()
        {
            Hero.AttackDPC();
        }
        internal void Update()
        {
            Hero.Update();
            // 엘레멘탈 매니져

            gameTime += Time.deltaTime;
            if (gameTime > testTimeTick)
            {
                gameTime -= testTimeTick;
                Hero.IncreaseDPC(3);
            }

        }
        internal void GainExp(BigInteger damage)
        {
            Hero.GainExp(damage);
        }

        // 히어로컨트롤러에들어가야할것들;
        internal void noticeDamageChanged()
        {
            string dpsString = "DPS : " + Singleton<ElementalManager>.getInstance().GetTotalDPS().ToSexyBackString() + "\n" + "DPC : " + Hero.GetTotalDPC().ToSexyBackString();
            label_herodmg.text = dpsString;
        }
        internal void noteiceExpChanged()
        {
            string expstring = Hero.EXP.ToSexyBackString() + " EXP";
            label_exp.text = expstring;
        }

    }
}