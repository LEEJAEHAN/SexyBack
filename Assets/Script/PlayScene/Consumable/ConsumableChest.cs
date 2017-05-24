using UnityEngine;

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

        }
    }
}