using UnityEngine;
using System;

namespace SexyBackPlayScene
{
    internal class ConsumableChest
    {
        private Consumable consumable;
        GameObject view;
        int Level;

        internal ConsumableChest(Consumable c, Vector3 dropWorldPosition, bool randmove, int level)
        {
            consumable = c;
            Level = level;
            view = ViewLoader.InstantiatePrefab(ViewLoader.battlearea.transform, "chest", "Prefabs/UI/ConsumableChest");
            view.transform.transform.OverlayPosition(dropWorldPosition, GameCameras.HeroCamera, GameCameras.UICamera);
            view.transform.position = new Vector3(view.transform.position.x, view.transform.position.y, 0f);
            NestedIcon.Draw(c.baseData.icon, view.transform.FindChild("Icon").gameObject);
            view.transform.FindChild("Icon/Stack").gameObject.GetComponent<UILabel>().text = StringParser.GetStackText(consumable.Stack);
            view.GetComponent<ConsumableChestView>().RandomMove = randmove;
            view.GetComponent<ConsumableChestView>().Action_Open += Add;
            view.GetComponent<ConsumableChestView>().Action_Destroy += Destory;
        }

        internal void Add()
        {
            switch(consumable.baseData.type)
            {
                case Consumable.Type.Exp:
                    BigInteger exp =  ConsumableManager.CalChestExp(Level, consumable.baseData.value);
                    Singleton<InstanceStatus>.getInstance().ExpGain(exp, false);
                    break;
                case Consumable.Type.Gem:
                    //TODO: 젬획득
                    //Singleton<PlayerStatus>.getInstance().
                    break;
                default:
                    Singleton<ConsumableManager>.getInstance().Stack(consumable);
                    break;
            }
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