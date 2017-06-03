using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackRewardScene
{
    public class ChestOpenScript : MonoBehaviour
    {
        bool isOpen = false;
        ChestSource Mode;

        private void Awake()
        {
            isOpen = false;
        }
        // Use this for initialization
        void Start()
        {

        }
        public void Open()
        {
            if (isOpen)
                return;

            if (Mode == ChestSource.GemOpen)        // 일단 나중에하자.
                return;
            else
            {
                transform.FindChild("Chest").GetComponent<UISprite>().spriteName = "Icon_30";
                transform.FindChild("Container/Source").gameObject.SetActive(false);
                transform.FindChild("Container/Item").gameObject.SetActive(true);
                transform.FindChild("Container/Item").gameObject.GetComponent<TweenAlpha>().PlayForward();
                Singleton<RewardManager>.getInstance().OpenNormalChest();
            }
        }
        // Update is called once per frame

        internal void SetMode(ChestSource mode)
        {
            Mode = mode;
            UILabel label = transform.FindChild("Container/Source/Normal").GetComponent<UILabel>();

            if (mode == ChestSource.Normal)
                label.text = "기본보상";
            else if (mode == ChestSource.Premium)
                label.text = "프리미엄보상";
            else if (mode == ChestSource.Event)
                label.text = "이벤트보상";
            else if (mode == ChestSource.Bonus)
                label.text = "추가보상";
            else if (mode == ChestSource.GemOpen)
            {
                transform.FindChild("Container/Source/Normal").gameObject.SetActive(false);
                transform.FindChild("Container/Source/GemOpen").gameObject.SetActive(true);
            }
        }

        internal void SetRank(RewardRank rank)
        {
            transform.FindChild("Chest/ChestRank").GetComponent<UISprite>().spriteName = "Rank_"+rank.ToString();
        }

        internal void SetEquip(Equipment value)
        {
            value.DrawIconView(transform.FindChild("Container/Item/Icon").GetComponent<UISprite>(),
                transform.FindChild("Container/Item/Name").GetComponent<UILabel>()
                , value.evolution);
        }
    }
}
