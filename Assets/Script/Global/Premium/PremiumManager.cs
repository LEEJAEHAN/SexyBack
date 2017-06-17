using System;
using SexyBackRewardScene;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Xml;
internal class PremiumManager
{
    public int Gem = 0;
    public bool PremiumUser = false;
    public List<string> PurchasedList;

    TopWindow topView;

    internal void NewData()    // 최초 게임 실행시 셋팅되는 장비.
    {
        Gem = 10000;
        NoticeJewel();
    }
    internal void BindTopView(TopWindow view)
    {
        topView = view;
    }
    private void NoticeJewel()
    {
        if (topView != null)
            topView.PrintSlot3String(Gem.ToString());
    }
    public void Load()
    {
        NewData();
    }
}