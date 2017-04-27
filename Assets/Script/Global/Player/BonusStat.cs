using System;

public class BonusStat : ICloneable
{
    public string targetID;  // both hero and elemental
    public string attribute;
    public int value;
    public string description;
    public string strvalue;

    public BonusStat(string targetID, string attribute, int value, string strvalue, string description)
    {
        this.targetID = targetID;
        this.attribute = attribute;
        this.value = value;
        this.strvalue = strvalue;
        this.description = description;
    }

    public object Clone()
    {
        return new BonusStat(this.targetID, this.attribute, this.value, this.strvalue, this.description);
    }
}