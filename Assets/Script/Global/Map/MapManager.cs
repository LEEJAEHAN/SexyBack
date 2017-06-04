using System;
using SexyBackRewardScene;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.SceneManagement;

internal class MapManager
{
    public Dictionary<string, Map> Maps;

    internal void InitOrLoad()
    {
        if (Maps != null)
            return;

        if (SaveSystem.GlobalDataExist)
        {
            sexybacklog.Console("PlayerStatus 파일로부터 로드.");
            Load();
        }
        else
        {
            sexybacklog.Console("PlayerStatus 시작초기 데이터로 생성.");
            NewData();
        }
    }

    private void Load()
    {
        XmlDocument doc = SaveSystem.LoadXml(SaveSystem.SaveDataPath);
        Maps = new Dictionary<string, Map>();

        foreach (MapData map in Singleton<TableLoader>.getInstance().mapTable)
        {
            Maps.Add(map.ID, new Map(map));
        }

        foreach (XmlNode node in doc.SelectSingleNode("PlayerStatus/Progress").ChildNodes)
        {
            string id = node.Attributes["id"].Value;
            Maps[id].ClearCount = int.Parse(node.Attributes["clearcount"].Value);
            Maps[id].BestTime = int.Parse(node.Attributes["besttime"].Value);
        }
    }

    public void NewData()
    {
        sexybacklog.Console("플레이씬에서 새 게임을 시작합니다. 더미스텟으로 시작합니다.");
        Maps = new Dictionary<string, Map>();

        foreach(MapData map in Singleton<TableLoader>.getInstance().mapTable)
        {
            Maps.Add(map.ID, new Map(map));
        }
    }

    internal void StartMap(string id)
    {
        Singleton<SexyBackPlayScene.InstanceStatus>.getInstance().InstanceMap = Maps[id];
        SceneManager.LoadScene("PlayScene");
    }
}

