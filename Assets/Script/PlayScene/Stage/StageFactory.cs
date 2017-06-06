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

            if (baseStage.type == StageType.Normal)
            {
                // 상태값.
                baseStage.rewardComplete = false;
                baseStage.MakeMonsters(StageManager.MonsterPerStage);
                baseStage.ChangeState("Move");
            }
            else if (baseStage.type == StageType.FirstPortal)
            {
                baseStage.rewardComplete = false;
                baseStage.ChangeState("Move");
            }
            else if (baseStage.type == StageType.LastPortal)
            {
                baseStage.rewardComplete = true;
                baseStage.ChangeState("Move");
            }
            return baseStage;
        }
        internal Stage LoadStage(Stage stagedata)
        {
            //Stage baseStage = MakeBaseStage(stagedata.zPosition, stagedata.floor);
            //baseStage.rewardComplete = stagedata.rewardComplete;
            //baseStage.monsters = stagedata.monsters;
            //baseStage.ChangeState(stagedata.savedState);
            //return baseStage;
            return null;
        }
        internal Stage LoadStage(XmlNode node)
        {
            float zPosition = float.Parse(node.Attributes["zPosition"].Value);
            int floor = int.Parse(node.Attributes["floor"].Value);
            Stage baseStage = MakeBaseStage(zPosition, floor);
            baseStage.rewardComplete = bool.Parse(node.Attributes["rewardcomplete"].Value);
            baseStage.monsterCount = int.Parse(node.Attributes["monstercount"].Value);
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

            if (floor >= 1 && floor <= StageManager.MaxFloor) // 1~ 10
            {
                result.type = StageType.Normal;
                result.avatar.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Stage/Stage_Castle");
            }
            else if (floor < 1) // 0
            {
                result.type = StageType.FirstPortal;
                result.avatar.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Stage/Stage_Entrance");
            }
            else if (floor > StageManager.MaxFloor) // 11
            {
                result.type = StageType.LastPortal;
                result.avatar.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Stage/Stage_Portal");
            }

            return result;
        }

    }
}
