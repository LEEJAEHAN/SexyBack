using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace SexyBackPlayScene
{
    internal class StageFactory
    {
        internal Stage CreateStage(int floor, float zPosition)
        {
            Stage baseStage = MakeBaseStage(zPosition, floor);
            //baseStage.rewardComplete = false;
            baseStage.ChangeState("Move");
            return baseStage;
        }
        internal Stage LoadStage(XmlNode node)
        {
            float zPosition = float.Parse(node.Attributes["zPosition"].Value);
            int floor = int.Parse(node.Attributes["floor"].Value);
            Stage baseStage = MakeBaseStage(zPosition, floor);
            //baseStage.monsterCount = int.Parse(node.Attributes["monstercount"].Value);
            baseStage.ChangeState(node.Attributes["state"].Value);
            return baseStage;
        }
        
        Stage MakeBaseStage(float zPosition, int floor)
        {
            Stage result = new Stage();
            result.zPosition = zPosition;
            result.floor = floor;
            result.avatar = ViewLoader.InstantiatePrefab(ViewLoader.stagepanel.transform, "Stage" + floor, "Prefabs/stage");
            result.avatar.transform.localPosition = GameCameras.EyeLine * (zPosition / 10);

            int maxFloor = Singleton<InstanceStatus>.getInstance().InstanceMap.baseData.MaxFloor;

            if (floor >= 1 && floor <= maxFloor) // 1~ 10
            {
                result.type = StageType.Normal;
                result.avatar.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Stage/Stage_Castle");
            }
            else if (floor < 1) // 0
            {
                result.type = StageType.FirstPortal;
                result.avatar.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Stage/Stage_Entrance");
            }
            else if (floor > maxFloor) // 11
            {
                result.type = StageType.LastPortal;
                result.avatar.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Stage/Stage_Portal");
            }

            return result;
        }

    }
}
