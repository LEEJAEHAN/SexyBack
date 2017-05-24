using UnityEngine;
using System;

namespace SexyBackPlayScene
{
    internal class ConsumableChest
    {
        private Consumable consumable;
        GameObject view;

        internal ConsumableChest(Consumable item, Vector3 dropPosition)
        {
            this.consumable = item;

            view = ViewLoader.InstantiatePrefab(ViewLoader.objectarea.transform, "chest", "Prefabs/UI/ConsumableChest");
            //ViewLoader.monsterbucket.transform.position
            view.transform.transform.OverlayPosition(dropPosition, GameCameras.HeroCamera, GameCameras.UICamera);
            view.transform.position = new Vector3(view.transform.position.x, view.transform.position.y, 0f );

            NestedIcon.Draw(item.baseData.icon, view.transform.FindChild("Icon").gameObject);
            view.transform.FindChild("Icon/Stack").gameObject.GetComponent<UILabel>().text = "x" + consumable.Stack.ToString();
            view.GetComponent<ConsumableChestView>().Action_Open += Add;
            view.GetComponent<ConsumableChestView>().Action_Destroy += Destory;
        }

        internal void Add()
        {
            Singleton<ConsumableManager>.getInstance().Stack(consumable);
        }

        internal void Destory()
        {
            Singleton<ConsumableManager>.getInstance().DestroyChest(this);
            view.GetComponent<ConsumableChestView>().Action_Open -= Add;
            view.GetComponent<ConsumableChestView>().Action_Destroy -= Destory;
            consumable = null;
            GameObject.Destroy(view);
        }
    }
}