using System;
using SexyBackRewardScene;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Xml;
internal class PremiumManager
{
    public int Jewel = 0;
    public bool PremiumUser = false;
    public List<string> PurchasedList;

    internal void Init()
    {
        // 어디서든 이닛될수 있으니 이미 이닛되있으면 리턴한다.
    }
}