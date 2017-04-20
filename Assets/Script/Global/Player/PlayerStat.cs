﻿using System;
using System.Runtime.Serialization;

[Serializable]
internal class PlayerStat
{
    internal int ResearchTime; 
    internal int ResearchThread;
    internal int ExpIncreaseXH;
    internal int LevelUpPriceXH;
    internal int ResearchPriceXH;
    internal BigInteger InitExp;

    // only use battleScene
    internal int ResearchTimeX; 


    public PlayerStat()
    {
        ResearchTimeX = 1;
        ResearchTime = 0;
        ResearchThread = 5; // for test
        ExpIncreaseXH = 100;
        LevelUpPriceXH = 100;
        ResearchPriceXH = 100;
        InitExp = new BigInteger(0);
    }

internal void Add(Bonus bonus)
    {
        switch (bonus.attribute)
        {
            case "ExpIncreaseXH":
                    ExpIncreaseXH += bonus.value;
                    break;
            case "ResearchTime":
                    ResearchTime += bonus.value;
                    break;
            case "ResearchThread":
                    ResearchThread += bonus.value;
                    break;
            case "LevelUpPriceXH":
                    LevelUpPriceXH -= bonus.value;
                    break;
            case "ResearchPriceXH":
                    ResearchPriceXH -= bonus.value;
                    break;
            case "InitExp":
                    InitExp += bonus.bigvalue;
                    break;
            default:
                {
                    sexybacklog.Error("noAttribute");
                    break;
                }
        }
    }
    internal void Remove(Bonus bonus)
    {
        switch (bonus.attribute)
        {
            case "ExpIncreaseXH":
                ExpIncreaseXH -= bonus.value;
                break;
            case "ResearchTime":
                ResearchTime -= bonus.value;
                break;
            case "ResearchThread":
                ResearchThread -= bonus.value;
                break;
            case "LevelUpPriceXH":
                LevelUpPriceXH += bonus.value;
                break;
            case "ResearchPriceXH":
                ResearchPriceXH += bonus.value;
                break;
            case "InitExp":
                InitExp -= bonus.bigvalue;
                break;
            default:
                {
                    sexybacklog.Error("noAttribute");
                    break;
                }
        }
    }

}