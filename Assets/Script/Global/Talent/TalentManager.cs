using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SexyBackMenuScene;
using System.Xml;

internal class TalentManager
{
    internal static int ReputationUnit = 50;

    public int Reputation;
    public int SpendReputation;
    public SortedDictionary<string, Talent> Talents;
    public Talent CurrentTalent;
    internal int TotalLevel { get { return Talents.Sum(item => item.Value.Level); } }
    
    TalentWindow talentView;
    InfoViewScript infoView;
    TopWindow topView;
    
    internal void InitOrLoad()
    {
        sexybacklog.Console("TalentManager 로드 및 초기화");
        if (CurrentTalent != null)
            return;

        if (SaveSystem.GlobalDataExist)
        {
            sexybacklog.Console("TalentManager 파일로부터 로드.");
            Load();
        }
        else
        {
            sexybacklog.Console("TalentManager 시작초기 데이터로 생성.");
            NewData();
        }
    }

    internal void NewData()    // 최초 게임 실행시 셋팅되는 장비.
    {
        Talents = new SortedDictionary<string, Talent>();
        var data = Singleton<TableLoader>.getInstance().talenttable["T01"];
        Talents.Add(data.ID, new Talent(data, 1));
        CurrentTalent = Talents["T01"];
        Reputation = 10000000;
        NoticeReputation();
    }
    public void Load()
    {
        XmlDocument doc = SaveSystem.LoadXml(SaveSystem.SaveDataPath);
        XmlNode mainNode = doc.SelectSingleNode("PlayerStatus/Talents");

        Reputation = int.Parse(mainNode.Attributes["Reputation"].Value);
        SpendReputation = int.Parse(mainNode.Attributes["SpendReputation"].Value);
        string currentID = mainNode.Attributes["CurrentTalent"].Value;

        Talents = new SortedDictionary<string, Talent>();
        var datas = Singleton<TableLoader>.getInstance().talenttable;
        foreach (XmlNode childNode in mainNode.ChildNodes)
        {
            string id = childNode.Attributes["id"].Value;
            int level = int.Parse(childNode.Attributes["level"].Value);
            Talents.Add(id, new Talent(datas[id], level));
        }

        CurrentTalent = Talents[currentID];
        NoticeReputation();
    }

    internal bool IsMaxLevel(string selectedID)
    {
        if (Talents.ContainsKey(selectedID))
            return Talents[selectedID].IsMaxLevel;
        else
            return false;
    }

    internal string GetTotalBonus()
    {
        string temp = "";

        temp += "[FF0000FF]" + StringParser.GetAttributeString(CurrentTalent.JobBonus) + "[-]\n";
        temp += "[0080FFFF]";
        foreach (var a in Talents.Values)
            temp += StringParser.GetAttributeString(a.TotalBonus) + "\n";
        temp += "[-]";
        return temp;
    }

    internal void Reset()
    {
        Reputation += SpendReputation;
        NewData();
        Singleton<PlayerStatus>.getInstance().Init();
        Singleton<PlayerStatus>.getInstance().ReCheckStat();
        Notice();
    }

    internal bool CanBuy(string selectedID)
    {
        return Reputation > GetNextPrice(selectedID);
    }

    public int GetNextPrice(string target)
    {
        return GetNextPrice(Singleton<TableLoader>.getInstance().talenttable[target].PriceCoef);
    }
    public int GetNextPrice(int priceCoef)
    {
        int LevelCoef = (int)Mathf.Pow(2, TotalLevel);
        return ReputationUnit * LevelCoef * priceCoef;
    }
    internal bool CanChangeJob(string selectedID)
    {
        if (CurrentTalent.ID == selectedID)
            return false;

        return Talents.ContainsKey(selectedID);
    }

    internal void JobChange(string selectedID)  // redrow
    {
        if (CurrentTalent != null)
            Singleton<PlayerStatus>.getInstance().ApplySpecialStat(CurrentTalent.JobBonus, false);
        CurrentTalent = Talents[selectedID];
        Singleton<PlayerStatus>.getInstance().ApplySpecialStat(CurrentTalent.JobBonus, true);

        Notice();
    }

    private void Notice()
    {
        NoticeReputation();
        talentView.RefreshList(Talents, TotalLevel);
        infoView.RefreshTalent(TotalLevel, CurrentTalent.Name, GetTotalBonus());
    }

    private void NoticeReputation()
    {
        if(topView!=null)
            topView.PrintSlot1String(Reputation.ToString());
    }

    internal void Learn(string selectedID) // redrow
    {
        if (Talents.ContainsKey(selectedID) == false)
        {
            Talent newOne = new Talent(Singleton<TableLoader>.getInstance().talenttable[selectedID], 0);
            Talents.Add(newOne.ID, newOne);
        }
        int price = GetNextPrice(Talents[selectedID].PriceCoef);
        Reputation -= price;
        SpendReputation += price;

        Talents[selectedID].Level++;
        Singleton<PlayerStatus>.getInstance().ApplySpecialStat(Talents[selectedID].BonusPerLevel, true);
        Notice();
    }

    public void ReCheckStat()
    {
        Singleton<PlayerStatus>.getInstance().ApplySpecialStat(CurrentTalent.JobBonus, true);
        foreach (Talent item in Talents.Values)
            Singleton<PlayerStatus>.getInstance().ApplySpecialStat(item.TotalBonus, true);
    }

    internal void BindTopView(TopWindow view)
    {
        topView = view;
    }
    internal void BindView(SexyBackMenuScene.TalentWindow view)
    {
        this.talentView = view;
    }
    internal void BindInfoView(InfoViewScript view)
    {
        infoView = view;
    }

}