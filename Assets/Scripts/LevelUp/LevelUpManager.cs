using System;
using System.Collections.Generic;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class LevelUpManager
    {
        List <LevelUpItem> items;
        Queue<LevelUpProcess> processqueue = new Queue<LevelUpProcess>();


        // view 관련 ( 이벤트핸들러)
        GameObject viewControllerObject = ViewLoader.LevelUpViewController;
        LevelUpViewController viewController;
        
        GameObject LevelUpItemViewPrefab = Resources.Load<GameObject>("Prefabs/UI/LevelUpItemView") as GameObject;


        internal void Init()
        {
            items = new List<LevelUpItem>();
            Load();

            viewController = viewControllerObject.GetComponent<LevelUpViewController>();
            viewController.Confirm += this.Confirm;
            viewController.FillInfoFunction += FillInfoView;

            foreach(LevelUpItem item in items)
                viewController.AddItemView(item, LevelUpItemViewPrefab);
        }
        void Load()
        {
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[0], "SexyBackIcon_FireElemental", 1, 100));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[1], "SexyBackIcon_WaterElemental", 1, 500));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[2], "SexyBackIcon_RockElemental", 1, 4000));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[3], "SexyBackIcon_ElectricElemental", 1, 37500));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[4], "SexyBackIcon_SnowElemental", 1, 450000));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[5], "SexyBackIcon_EarthElemental", 1, 7250000));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[6], "SexyBackIcon_AirElemental", 1, 150000000));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[7], "SexyBackIcon_IceElemental", 1, 4000000000));
            items.Add(new LevelUpItem(Singleton<ElementalManager>.getInstance().elementals[8], "SexyBackIcon_MagmaElemental", 1, 140000000000));
        }

        internal void Update()
        {
            /// ui에서 confirm 클릭이들어오면 flag를 변경해놓을것이다.
            if(processqueue.Count > 0)
            {
                processqueue.Dequeue().LevelUp();
            }

            foreach (LevelUpItem item in items)
            {
                item.ApplyProcess(); // 업한 level을 elemental에 적용시키는것;
            }
        }

        public List<LevelUpItem> GetItemList()
        {
            return items;
        }
        private LevelUpItem FindItem(string id)
        {
            foreach (LevelUpItem item in items)
            {
                if (item.ItemViewID == id)
                    return item;
            }
            return null;
        }

        internal void onDpsChanged(Elemental sender)
        {
            // updateitemview
            //GameObject item = FindItemView(sender.ItemViewID);
            //GameObject label = item.transform.FindChild("Label").gameObject;
            //label.GetComponent<UILabel>().text = sender.Dps.ToSexyBackString();

            // UpdateInfoView if selected
            // 구현해야함
        }

        internal void onExpChanged()
        {
            // 구현해야함
            // 업글할 수 있는것만 것만 표시;
            // confirm 버튼 활성화 비활성화;
        }

        ///  이하는 view 표시 관련, view 단의 작업. 절대 data를 변경해선 안된다. ( 데이터는 update에서 변경되어야 한다.)

        public void Confirm(string selectedItemID)
        { // 모델을 변경시키는 행위기 때문에 실제 변경은 업데이트에서 수행되어야함;
            LevelUpProcess process = new LevelUpProcess(FindItem(selectedItemID), true);
            processqueue.Enqueue(process);
        }


        void FillInfoView(string levelupitemid)
        {
            LevelUpItem target = FindItem(levelupitemid);
            ViewLoader.Info_Icon.GetComponent<UISprite>().spriteName = target.IconName;
            ViewLoader.Info_Description.GetComponent<UILabel>().text = target.Description;
            //    ViewLoader.Info_Description
        }



    }
}